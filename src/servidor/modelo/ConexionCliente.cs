using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServidorChat.Modelo;

/// <summary>
/// Representa las conexiones 
/// TCP de los clientes del sistema de chat.
/// </summary>
public class ConexionCliente
{

    /// <summary>
    /// Atributo TcpClient que proporciona conexiones de cliente para los servicios de red TCP
    /// </summary>
    private readonly TcpClient tcpCliente;

    /// <summary>
    /// Atributo NetworkStream que proporciona el flujo de datos para el acceso a la red
    /// </summary>
    private readonly NetworkStream stream;

    public event Action<ConexionCliente, String>? mensajeRecibido;

    /// <summary>
    /// Atributo string que representa el nombre de usuario de la conexion
    /// </summary>
    private string username;

    /// <summary>
    /// Atributo string que representa el estado de la conexion
    /// </summary>
    private string status;

    /// <summary>
    /// Metodo para obtener el estado de la conexion
    /// </summary>
    /// <returns> el estado de la conexion </returns>
    public string GetStatus()
    {
        return this.status;
    }

    /// <summary>
    /// Metodo para asignar el estado de la conexion
    /// <param name="status"> Es el estado que queremos asignar</param>
    /// </summary>
    public void SetStatus(string status)
    {
        this.status = status;
    }

    /// <summary>
    /// Metodo para obtener el nombre de la conexion
    /// </summary>
    /// <returns> el nombre de la conexion</returns>
    public string GetUsername()
    {
        return this.username;
    }

    /// <summary>
    /// Metodo para asignar el nombre de la conexion
    /// <param name="username"> Es el nombre que queremos asignar</param>
    /// </summary>
    public void SetUsername(string username)
    {
        this.username = username;
    }

    /// <summary>
    /// Constructor de la clase ConexionCliente
    /// </summary>
    /// <param name="tcpCliente"> Es la conexion de cliente con la que trabajeremos</param>
    public ConexionCliente(TcpClient tcpCliente)
    {
        this.tcpCliente = tcpCliente;
        this.stream = tcpCliente.GetStream();
    }

    /// <summary>
    /// Metodo para desconectar la conexion del servidor, cerramos el NetWorkStream y el TcpClient
    /// </summary>
    public void Desconectar()
    {
        /* Cerramos el networkstream*/
        stream.Close();
        /* Cerramos el tcpClient*/
        tcpCliente.Close();
    }

    /// <summary>
    /// Metodo por el cual recibimos mensajes de manera asincrona
    /// </summary>
    /// <returns> Una tarea que representa el recibir mensajes</returns>
    public async Task RecibirMensajes()
    {
        /** Aqui es donde guardaremos el flujo de bytes que nos mande el cliente*/
        byte[] buffer = new byte[1024*1024];

        /* StringBuilder que nos ayuda a juntar las lineas*/
        StringBuilder acumulador = new StringBuilder();

        while (true)
        {
            /** Representa bytes leidos*/
            int bytesLeidos;

            try
            {
                /** leemos los bytes de manera asincrona y los guardamos en bytesLeidos */
                bytesLeidos = await stream.ReadAsync(buffer);
            }
            catch (Exception)
            {
                break;
            }

            if (bytesLeidos == 0)
            {
                break;
            }

            /* guardamos en una cadena los bytes codificados usando el formato UTF8*/
            string datosRecibidos = Encoding.UTF8.GetString(buffer, 0, bytesLeidos);

            /* Juntamos los datos recibidos*/
            acumulador.Append(datosRecibidos);

            /* pasamos esos datos a un string*/
            string contenido = acumulador.ToString();

            /* Separamos esos strings cada que encuentre un \n */
            string[] mensajes = contenido.Split('\n');

            acumulador.Clear();

            /* pegamos por si el ultimo elemento es un mensaje icompleto*/
            acumulador.Append(mensajes[mensajes.Length - 1]);

            /* recorremos los mensajes completos*/
            for (int i = 0; i < mensajes.Length - 1; i++)
            {
                /* obtenemos el actuual*/
                string mensaje = mensajes[i];

                /* vemos que no esta vacio*/
                if (!string.IsNullOrEmpty(mensaje))
                {
                    /* invocamos que s genero un mensaje*/
                    mensajeRecibido?.Invoke(this, mensaje);
                }
            }
        }
    }

    /// <summary>
    /// Metodo para enviar un mensaje al cliente, delimitamos con \n como dice el protocolo
    /// </summary>
    /// <param name="mensaje"> es el mensaje que queremos enviar </param>
    /// <returns> tarea que representa la operacion de enviar el mensaje </returns>
    public async Task EnviarMensaje(string mensaje)
    {

        mensaje += "\n";

        byte[] datosAEnviar = Encoding.UTF8.GetBytes(mensaje);

        await stream.WriteAsync(datosAEnviar, 0, datosAEnviar.Length);

    }
}