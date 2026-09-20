namespace protocoloMensajes;

/// <summary>
/// Clase que hereda de mensaje que representa 
/// la peticion de desconeccion
/// </summary>
public class Disconnect : Mensaje
{
    /// <summary>
    /// Constructor de la clase Disconnect
    /// </summary>
    public Disconnect()
    {
        type = "DISCONNECT";
    }


}