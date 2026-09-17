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

    private string username;

    private string status;

    public string GetStatus()
    {
        return this.status;
    }

    public void SetStatus(string status)
    {
        this.status = status;
    }

    public string GetUsername()
    {
        return this.username;
    }

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
        byte[] buffer = new byte[1024];

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

    public async Task EnviarMensaje(string mensaje)
    {

        mensaje += "\n";

        byte[] datosAEnviar = Encoding.UTF8.GetBytes(mensaje);

        await stream.WriteAsync(datosAEnviar, 0, datosAEnviar.Length);

    }
}