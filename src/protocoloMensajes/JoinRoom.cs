namespace protocoloMensajes;

/// <summary>
/// Clase que hereda de mensaje que representa el unirse
/// a un cuarto
/// </summary>
public class JoinRoom : Mensaje
{
    /// <summary>
    /// Atributo que representa el nombre del cuarto
    /// </summary>
    /// <value></value>
    public string roomname{get; set;}

    /// <summary>
    /// Constructor de la clase JoinRoom
    /// </summary>
    /// <param name="roomname"> es el nombre del cuarto </param>
    public JoinRoom(string roomname)
    {
        type = "JOIN_ROOM";
        this.roomname = roomname;
    }








}