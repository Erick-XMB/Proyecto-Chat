
using ServidorChat.Modelo;
using System.Text.Json;

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
    /// la lista con las conexiones y hcemos la suscripcion de una nueva conexion con su metodo
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
    /// Metodo que nos muetsra el mensaje recibido
    /// </summary>
    /// <param name="mensaje"> El mensaje que queremos mostrar</param>
    public async void MensajeRecibido(ConexionCliente cliente, string mensaje)
    {
        Identify identify = JsonSerializer.Deserialize<Identify>(mensaje);

        bool repetido = false;

        if (identify.type.Equals("IDENTIFY"))
        {

            foreach (ConexionCliente c in clientes)
            {
                if (c.getUsername() == identify.username)
                {
                    repetido = true;

                    UserAlreadyExist respuesta = new UserAlreadyExist(identify.username);

                    string json = JsonSerializer.Serialize(respuesta);

                    await cliente.EnviarMensaje(json);

                    break;
                }
            }

            if (!repetido)
            {
                cliente.setUsername(identify.username);
                string identifyMostrar = JsonSerializer.Serialize(identify);

                // { "type": "IDENTIFY","username": "Kimberly" }
                Console.WriteLine(identifyMostrar);
            }
        }
    }

    public void MostrarListaDeCliente()
    {
        foreach (ConexionCliente c in clientes)
        {
            Console.WriteLine(c.getUsername + "\n");
        }
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

