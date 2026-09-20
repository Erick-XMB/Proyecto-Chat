namespace protocoloMensajes;
/// <summary>
/// Clase que hereda de mensaje que representa 
/// quien envia un texto al cuarto 
/// </summary>
public class RoomTextFrom : Mensaje
{   
    /// <summary>
    /// Atibuto que representa el nombre de la sala
    /// </summary>
    public string roomname {get; set;}

    /// <summary>
    /// Atirbuto que representa quien envia el mensaje
    /// </summary>
    public string username {get; set;}

    /// <summary>
    /// Atirbuto que representa el mensaje
    /// </summary>
    public string text {get; set;}

    /// <summary>
    /// Constructor de la clase Room text from
    /// </summary>
    /// <param name="roomname"> es el nombre del cuarto</param>
    /// <param name="username"> es el nombre de usuario</param>
    /// <param name="text"> es el mensaje </param>
    public RoomTextFrom(string roomname, string username, string text)
    {   
        type = "ROOM_TEXT_FROM";
        this.roomname = roomname;
        this.username = username;
        this.text = text;
    }
}