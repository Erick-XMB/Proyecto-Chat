namespace protocoloMensajes;

/// <summary>
/// Clase que hereda de Mensaje que representa el mensaje
/// que alguien se unio al cuarto
/// </summary>
public class JoinedRoom : Mensaje
{   
    /// <summary>
    /// Atributo que representa el nombre del cuarto
    /// </summary>
    /// <value></value>
    public string roomname {get; set;}

    /// <summary>
    /// Atributo que representa el nombre de quien se une al cuarto
    /// </summary>
    /// <value></value>
    public string username {get; set;}

    /// <summary>
    /// Constructor de la clase JoinedRoom
    /// </summary>
    /// <param name="roomname"> es el nombre del cuarto </param>
    /// <param name="username"> es el nombre de quien se une al cuarto </param>
    public JoinedRoom(string roomname, string username)
    {
        type = "JOINED_ROOM";
        this.roomname = roomname;
        this.username = username;
    }
}