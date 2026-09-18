
using ServidorChat.Modelo;
using System.Text;
using System.Text.Json;
using protocoloMensajes;

namespace ServidorChat.controlador;
/// <summary>
/// Clase que representa el controlador de nuestro servidor
/// </summary>
public class ServidorControlador
{
    /// <summary>
    /// Atributo servidor que representa a un Servidor
    /// </summary>
    private readonly Servidor servidor;

    /// <summary>
    /// Lista clientes que es donde se guardan las conexiones de clientes que hagamos
    /// </summary>    
    private readonly List<ConexionCliente> clientes;

    /// <summary>
    /// Constructor de nuestro ServidorControlador, donde iniciamos el servidor
    /// la lista con las conexiones y hcemos la suscripcion de una nueva conexion con su metodos
    /// asignado
    /// </summary>
    /// <param name="puerto"> el puerto del servido. </param>
    public ServidorControlador(int puerto)
    {
        servidor = new Servidor(puerto);
        clientes = new List<ConexionCliente>();

        servidor.nuevaConexion += NuevaConexion;
    }


    /// <summary>
    /// Metodo donde obtenemos el atributo servidor de nuestra clase
    /// </summary>
    /// <returns> el atributo servidor </returns>
    public Servidor GetServidor()
    {
        return this.servidor;
    }


    /// <summary>
    /// Metodo asincrono que maneja las nuevas conexiones que se hagan al servidor
    /// </summary>
    /// <param name="cliente"></param>
    /// <returns></returns>
    public async void NuevaConexion(ConexionCliente cliente)
    {
        /** Agregamos la conexion a nuestra lista*/
        clientes.Add(cliente);

        /** suscribimos el metodo MensajeRecibido al evento de mensajeRecibido*/
        cliente.mensajeRecibido += MensajeRecibido;

        /** esperamos a que el cliente reciba mensajes*/
        await cliente.RecibirMensajes();
    }

    /// <summary>
    /// Metodo que procesa los dsitintos tipos de mensajes que podamos recibir
    /// </summary>
    /// <param name="cliente"> Es la conexion cliente que envio el mensaje a</param>
    /// <param name="mensaje"> El mensaje que queremos mostrar</param>
    public async void MensajeRecibido(ConexionCliente cliente, string mensaje)
    {

        try
        {
            Mensaje mensajeBase = JsonSerializer.Deserialize<Mensaje>(mensaje);

            switch (mensajeBase.type)
            {
                case "IDENTIFY":
                    ProcesarIdentify(cliente, mensaje);
                    break;

                case "STATUS":
                    ProcesarStatus(cliente, mensaje);
                    break;
                case "USERS":
                    ProcesarUsers(cliente, mensaje);
                    break;

                default:
                    ProcesarNotIdentify(cliente);
                    break;
            }
        }
        catch (JsonException)
        {
            ProcesarNotIdentify(cliente);
        }
    }

    /// <summary>
    /// Metodo privado que procesa la intruccion de IDENTIFY enviada al servidor
    /// Verifica que el nombre de usuario no este registrado, en otro casom registra al cliente y notifica la conexion
    /// </summary>
    /// <param name="cliente"> Es la conexion cliente que envio el mensaje a</param>
    /// <param name="mensaje"> Es el mensaje recibido en formato json </param>
    private async void ProcesarIdentify(ConexionCliente cliente, string mensaje)
    {
        Identify identify = JsonSerializer.Deserialize<Identify>(mensaje);

        bool repetido = false;

        foreach (ConexionCliente c in clientes)
        {
            if (c.GetUsername() == identify.username)
            {
                repetido = true;

                UserAlreadyExist respuesta = new UserAlreadyExist(identify.username);

                string jsonUserAlreadyExists = JsonSerializer.Serialize(respuesta);

                // respuesta del servidor
                await cliente.EnviarMensaje(jsonUserAlreadyExists);

                clientes.Remove(cliente);

                cliente.Desconectar();

                break;
            }
        }

        if (!repetido)
        {
            cliente.SetUsername(identify.username);

            Response respuesta = new Response();
            respuesta.operation = "IDENTIFY";
            respuesta.result = "SUCCESS";
            respuesta.extra = identify.username;

            NewUser newUser = new NewUser(identify.username);

            string jsonRespuesta = JsonSerializer.Serialize(respuesta);

            string identifyMostrar = JsonSerializer.Serialize(identify);

            string newUserMostrar = JsonSerializer.Serialize(newUser);

            await cliente.EnviarMensaje(identifyMostrar);
            await cliente.EnviarMensaje(jsonRespuesta);

            foreach (ConexionCliente c in clientes)
            {
                await c.EnviarMensaje(newUserMostrar);
            }

            Console.WriteLine(identifyMostrar);
        }
    }

    /// <summary>
    /// Metodo privado que procesa la intruccion de STATUS enviada al servidor
    /// Asigamos el status a nuestro cliente y motrsamos el mensaje de status
    /// </summary>
    /// <param name="cliente"> Es la conexion cliente que envio el mensaje a</param>
    /// <param name="mensaje"> Es el mensaje recibido en formato json </param>
    private void ProcesarStatus(ConexionCliente cliente, string mensaje)
    {
        Status status = JsonSerializer.Deserialize<Status>(mensaje);
        cliente.SetStatus(status.status);

        String mostrarStatus = JsonSerializer.Serialize(status);

        Console.WriteLine(mostrarStatus);
    }

    /// <summary>
    /// Metodo privado que procesa la intruccion de USERS enviada al servidor
    /// Mostramos el mensaje USERS, creamos un diccionario que contenga entrada de username y status
    /// de cada cliente en la lista de clientes obtenemos su nombre y su status y lo agregamos al diccionario
    /// Serializamos como UserList y lo regresamos al cliente
    /// 
    /// </summary>
    /// <param name="cliente"> Es la conexion cliente que envio el mensaje a</param>
    /// <param name="mensaje"> Es el mensaje recibido en formato json </param>
    private async void ProcesarUsers(ConexionCliente cliente, string mensaje)
    {
        Users usuario = JsonSerializer.Deserialize<Users>(mensaje);

        string mostrarUser = JsonSerializer.Serialize(usuario);

        Console.WriteLine(mostrarUser);

        Dictionary<string, string> users = new Dictionary<string, string>();

        foreach (ConexionCliente c in clientes)
        {
            string username = c.GetUsername();
            string status = c.GetStatus();

            users.Add(username, status);
        }

        UserList userList = new UserList(users);

        string jsonUserList = JsonSerializer.Serialize(userList);

        await cliente.EnviarMensaje(jsonUserList);
    }

    /// <summary>
    /// Metodo privado que procesa los mensajes distintos a Identify enviados al servidor
    /// Creamos el mensaje notIdentify lo serializamos y lo regresamos al cliente
    /// </summary>
    /// <param name="cliente"> Es la conexion cliente que envio el mensaje a</param>
    private async void ProcesarNotIdentify(ConexionCliente cliente)
    {
        NotIdentify notIdentify = new NotIdentify();
        string jsonNotIdentify = JsonSerializer.Serialize(notIdentify);
        await cliente.EnviarMensaje(jsonNotIdentify);
    }


    /// <summary>
    /// Operacion asincrona que nos permite iniciar le servidor
    /// </summary>
    /// <returns> Una tarea que representa la operacion asincrona de inicio del servidor </returns>
    public async Task Iniciar()
    {
        await servidor.Iniciar();
    }

    /// <summary>
    /// Metodo por el cual cerramos el servidor
    /// </summary>
    public void Cerrar()
    {
        servidor.CerrarPuerto();
    }


}

