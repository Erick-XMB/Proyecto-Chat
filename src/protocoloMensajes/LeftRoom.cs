namespace protocoloMensajes;

/// <summary>
/// Clase que hereda de Mensaje que representa la operacion
/// de avisar que alguien abandono un cuarto
/// </summary>
public class LeftRoom : Mensaje
{
    /// <summary>
    /// Es el nombre de la sala
    /// </summary>
    /// <value></value>
    public string roomname{get; set;}

    /// <summary>
    /// Es el nombre de quien abandona la sala
    /// </summary>
    public string username {get; set;}

    /// <summary>
    /// Constructor de la clase LeftRoom
    /// </summary>
    /// <param name="roomname"> es el nombre del cuarto </param>
    /// <param name="username"> es el nombre de usuario </param>
    public LeftRoom(string roomname, string username)
    {
        type = "LEFT_ROOM";
        this.roomname = roomname;
        this.username = username;
    }


}