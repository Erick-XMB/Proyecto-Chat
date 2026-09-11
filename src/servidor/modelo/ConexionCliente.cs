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

    public event Action<String>? mensajeRecibido;

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
    /// Metodo por el cual recibimos mensajes de manera asincrona
    /// </summary>
    /// <returns> Una tarea que representa el recibir mensajes</returns>
    public async Task RecibirMensajes()
    {   
        /** Aqui es donde guardaremos el flujo de bytes que nos mande el cliente*/
        byte[] buffer = new byte[1024];

        while (true)
        {   
            /** Representa bytes leidos*/
            int bytesLeidos;

            try
            {

                /** leemos los bytes de manera asincrona y los guardamos en bytesLeidos */
                bytesLeidos = await stream.ReadAsync(buffer);

                /** guardamos en una cadena los bytes codificados usando el formato UTF8*/
                string mensaje = Encoding.UTF8.GetString(buffer, 0, bytesLeidos);

                /**Disparamos el evento de mensajeRecibido usando como parametro el mensaje */
                mensajeRecibido?.Invoke(mensaje);
            }
            catch (Exception)
            {
                break;
            }

            if (bytesLeidos == 0)
            {
                break;
            }
        }
    }




}