using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClienteChat.modelo;

/// <summary>
/// Clase que representa las conexiones que el cliente tiene con el servidor
/// </summary>
public class ConexionServidor
{

    /** Checa el estado de la conexion TCP*/
    private TcpClient? cliente;

    /** Atributo que maneja el flujo de la lectura*/
    private NetworkStream? stream;

    /// <summary>
    /// Evento que invoca a la accion de la cadena que le llamamos mensaje recibido
    /// </summary>
    public event Action<string>? MensajeRecibido;


    /// <summary>
    /// Metodo que nos permite conectarnos al servidor
    /// </summary>
    /// <returns>
    /// <c>true</c> si se pudo establecer conexion, en caso contrario, <c>false</c>.
    /// </returns>
    public bool Conectar(int puerto)
    {
        /** Esta es la drieccion de servdior, usamos la del localhost*/
        string servidor = "127.0.0.1";

        try
        {
            /** CReamos un objeto TcpClient con el servidor y puerto previamente definidos*/
            cliente = new TcpClient(servidor, puerto);

            /** Este es el canal de comunicacion asociado al socket TCP*/
            stream = cliente.GetStream();

            return true;

        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <summary>
    /// Metodo que nos permite desonectar cerrando el TCPClient y el NetWorkStream
    /// </summary>
    public void Desconectar()
    {
        stream?.Close();
        cliente?.Close();
    }

    /// <summary>
    /// Metodo que envia un mensaje al servidor usando la conexion de TCP
    /// </summary>
    /// <param name="mensaje">
    /// texto que se quiere enviar al servidor
    /// </param>
    public async void EnviarMensaje(string mensaje)
    {
        /** Aqui se verifica que haya una conexionn establecida con el servidor
            pues si es null siginifica que todavia no hay conexion*/
        if (stream == null)
        {
            return;
        }

        mensaje += "\n";

        /** Aqui se conierten una cadena de texto en un arreglos de numero binarios
            con el formato UTF8*/
        byte[] datos = Encoding.UTF8.GetBytes(mensaje);

        /** Enviamos los bytes que tenemos en datos usando el NetWorkStream*/
        stream.Write(datos, 0, datos.Length);
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
                if (stream == null)
                {
                    return;
                }

                /** leemos los bytes de manera asincrona y los guardamos en bytesLeidos */
                bytesLeidos = await stream.ReadAsync(buffer);


                /** guardamos en una cadena los bytes codificados usando el formato UTF8*/
                string mensaje = Encoding.UTF8.GetString(buffer, 0, bytesLeidos);

                MensajeRecibido?.Invoke(mensaje);

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








