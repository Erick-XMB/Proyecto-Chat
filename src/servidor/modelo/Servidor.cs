using System.Net;
using System.Net.Sockets;

namespace ServidorChat.Modelo;


/// <summary>
/// Clase que representa el servidor encargado de aceptar y administrar las conexiones
/// de los clientes del sistema de chat.
/// </summary>
public class Servidor
{
    /// <summary>
    /// Atibuto TcpListener que hace referencia al servidor
    /// </summary>
    private readonly TcpListener listener;

    /// <summary>
    /// Atributo entero que hace referencia al puerto de servidor
    /// </summary>
    private readonly int puerto;

    /// <summary>
    /// Atibuto booleando que hace referencia a si esta activo el servidor
    /// </summary>
    private bool activo;


    /// <summary>
    /// Evento que notifica cuando se establece una nueva conexion con un cliente
    /// </summary>
    public event Action<ConexionCliente>? nuevaConexion;


    /// <summary>
    /// Metodo que devuelve el atributo activo del sevidor
    /// </summary>
    /// <returns>El estado activo del servidor</returns>
    public bool GetActivo()
    {
        return this.activo;
    }

    /// <summary>
    /// Constructor de la clase Servidor
    /// </summary>
    /// <param name="puerto">Numero entero que representa el puerto del servidor.</param>
    public Servidor(int puerto)
    {
        this.puerto = puerto;
        listener = new TcpListener(IPAddress.Any, puerto);
    }

    /// <summary>
    /// Metodo que activa el servidor, inicia el TcpListener y comienza a aceptar clientes de manera asincrona
    /// </summary>
    /// <returns> Una tarea que representa la operacion asincorna de aceptar clientes</returns>
    public async Task Iniciar()
    {
        /** Pasamos el atributo activo a verdadero*/
        activo = true;

        /** Usamos la funcion Start() que nos proporciona TcpListener para iniciar el servidor*/
        listener.Start();

        Console.WriteLine("Servidor se inicio");

        /** Esperamos a que aceptemos los clientes*/
        await AceptarClientesTCPdeModoAsincrono();
    }

    /// <summary>
    /// Metodo que desactiva el servidor y detiene al TcpListener
    /// </summary>
    public void CerrarPuerto()
    {
        /** Pasamos el atributo de activo a falso */
        activo = false;

        /** Usamos la funcion Stop() que nos proporciona TcpListener para detener el servidor */
        listener.Stop();
    }

    /// <summary>
    /// Metodo privado que acepta clientes TCP de manera asincrona
    /// y crea una instancia ConexionCliente para cada nueva conexion
    /// </summary>
    /// <returns> Una tarea que representa la operacion asincrona de aceptar clientes. </returns>
    private async Task AceptarClientesTCPdeModoAsincrono()
    {
        /** Nos aseguramos que el servidor en efecto este activo*/
        activo = true;
        while (activo)
        {
            try
            {
                /** Creamos una instancia de TcpClient que recibira al cliente que estamos
                    aceptando de manera asincrona*/
                TcpClient tcpCliente = await listener.AcceptTcpClientAsync();

                /** Creamos una instancia de ConexionCliente usando al TcpClient que recuperamos*/
                ConexionCliente conexionNueva = new ConexionCliente(tcpCliente);

                /** Disparamos el evento nuevaConexion pasando la conexionNueva*/
                nuevaConexion?.Invoke(conexionNueva);
            }
            catch (Exception) when (!activo)
            {
                break;
            }
        }
    }
}






