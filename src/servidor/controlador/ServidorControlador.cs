
using ServidorChat.Modelo;
using System.Text;
using System.Text.Json;
using protocoloMensajes;
using Microsoft.VisualBasic.FileIO;

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

    public void CerrarConexion(ConexionCliente cliente)
    {
        clientes.Remove(cliente);
        cliente.Desconectar();
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
                case "PUBLIC_TEXT":
                    ProcesarPublicText(cliente, mensaje);
                    break;
                case "TEXT":
                    ProcesarText(cliente, mensaje);
                    break;

                default:
                    if (mensajeBase.type != "IDENTIFY" && mensajeBase.type != null)
                    {
                        ProcesarNotIdentify(cliente);
                        CerrarConexion(cliente);
                    }
                    else
                    {
                        ProcesarJsonNoValido(cliente);
                        CerrarConexion(cliente);
                    }
                    break;
            }
        }
        catch (JsonException)
        {
            ProcesarJsonNoValido(cliente);
            CerrarConexion(cliente);
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

                CerrarConexion(cliente);

                break;
            }
        }

        if (!repetido)
        {
            cliente.SetUsername(identify.username);

            cliente.SetStatus("ACTIVE");

            Response respuesta = new Response();
            respuesta.operation = "IDENTIFY";
            respuesta.result = "SUCCESS";
            respuesta.extra = identify.username;

            string jsonRespuesta = JsonSerializer.Serialize(respuesta);

            string identifyMostrar = JsonSerializer.Serialize(identify);

            ProcesarNewUser(cliente);

            await cliente.EnviarMensaje(jsonRespuesta);

            Console.WriteLine(identifyMostrar);
        }
    }


    private async void ProcesarNewUser(ConexionCliente cliente)
    {
        NewUser newUser = new NewUser(cliente.GetUsername());
        string jsonNewUser = JsonSerializer.Serialize(newUser);

        foreach (ConexionCliente c in clientes)
        {
            if (c != cliente)
            {
                await c.EnviarMensaje(jsonNewUser);
            }
        }
    }
    /// <summary>
    /// Metodo privado que procesa la intruccion de STATUS enviada al servidor
    /// Asigamos el status a nuestro cliente y motrsamos el mensaje de status
    /// </summary>
    /// <param name="cliente"> Es la conexion cliente que envio el mensaje a</param>
    /// <param name="mensaje"> Es el mensaje recibido en formato json </param>
    private async void ProcesarStatus(ConexionCliente cliente, string mensaje)
    {
        Status status = JsonSerializer.Deserialize<Status>(mensaje);
        cliente.SetStatus(status.status);

        NewStatus newStatus = new NewStatus(cliente.GetUsername(), cliente.GetStatus());

        string jsonNewStatus = JsonSerializer.Serialize(newStatus);

        foreach (ConexionCliente c in clientes)
        {
            if (c != cliente)
            {

                await c.EnviarMensaje(jsonNewStatus);
            }
        }

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

    private async void ProcesarPublicText(ConexionCliente cliente, string mensaje)
    {
        Console.WriteLine(mensaje);

        PublicText publicText = JsonSerializer.Deserialize<PublicText>(mensaje);

        PublicTextFrom publicTextFrom = new PublicTextFrom(cliente.GetUsername(), publicText.text);

        string jsonPublicTextFrom = JsonSerializer.Serialize(publicTextFrom);

        foreach (ConexionCliente c in clientes)
        {
            if (c != cliente)
            {
                await c.EnviarMensaje(jsonPublicTextFrom);
            }
        }
    }

    private async void ProcesarText(ConexionCliente cliente, string mensaje)
    {
        // recibimos el mensaje de text
        Console.WriteLine(mensaje);
        PrivText privText = JsonSerializer.Deserialize<PrivText>(mensaje);

        // guardamos la informacion
        string usernameDestino = privText.username;
        string usernameOrigen = cliente.GetUsername();
        string textoDelMensaje = privText.text;

        // creamos el textFrom
        PrivTextFrom privTextFrom = new PrivTextFrom(usernameOrigen, textoDelMensaje);
        string jsonPrivTextFrom = JsonSerializer.Serialize(privTextFrom);

        bool usuarioEncontrado = false;

        // enviamos cmo respuesta el PrivTextFrom
        foreach (ConexionCliente c in clientes)
        {
            if (c.GetUsername() == usernameDestino)
            {
                await c.EnviarMensaje(jsonPrivTextFrom);
                usuarioEncontrado = true;
                break;
            }
        }

        // si el usuario no esta en las conexiones
        if (!usuarioEncontrado)
        {
            NoSuchUser noSuchUser = new NoSuchUser(usernameDestino);
            string jsonNoSuchUser = JsonSerializer.Serialize(noSuchUser);
            await cliente.EnviarMensaje(jsonNoSuchUser);
        }
    }

    private async void ProcesarJsonNoValido(ConexionCliente cliente)
    {
        Response response = new Response();
        response.operation = "INVALID";
        response.result = "INVALID";

        string jsonNovalido = JsonSerializer.Serialize(response);
        await cliente.EnviarMensaje(jsonNovalido);
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

