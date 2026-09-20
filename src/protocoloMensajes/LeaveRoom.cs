namespace protocoloMensajes;

/// <summary>
/// Clase que hereda de Mensaje que representa la peticion
/// de abandonar una sala
/// </summary>
public class LeaveRoom : Mensaje
{
    /// <summary>
    /// Atributo que representa el nombre de la sala
    /// </summary>
    public string roomname {get; set;}


    /// <summary>
    /// Constructor de la clase LeaveRoom
    /// </summary>
    /// <param name="roomname"> es el nombre de la sala</param>
    public LeaveRoom(string roomname)
    {
        type = "LEAVE_ROOM";
        this.roomname = roomname;
    }


}