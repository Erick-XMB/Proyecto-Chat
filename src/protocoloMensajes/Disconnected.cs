namespace protocoloMensajes;
/// <summary>
/// Clase que hereda de mensaje que representa el mensaje
/// que alguien se desconecto
/// </summary>
public class Disconnected : Mensaje
{
    /// <summary>
    /// 
    /// Atributo que representa el nombre de usuario de un cliente
    /// </summary>
    public string username { get; set; }


    /// <summary>
    /// Constructor de la clase Disconnected
    /// </summary>
    /// <param name="username"> es el nombre de usuario </param>
    public Disconnected(string username)
    {
        type = "DISCONNECTED";
        this.username = username;
    }

}