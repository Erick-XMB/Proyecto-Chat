namespace protocoloMensajes;

/// <summary>
/// Clase que hereda de Mensaje que representa
/// la operacion de ROOM_USERS 
/// </summary>
public class RoomUsers : Mensaje
{   
    /// <summary>
    /// Atirbuto que representa el nombre de la sala
    /// </summary>
    public string roomname{get; set;}

    /// <summary>
    /// Constructor de la clase RoomUsers
    /// </summary>
    /// <param name="roomname"> es el nombre de la sala </param>
    public RoomUsers(string roomname)
    {
        type = "ROOM_USERS";
        this.roomname = roomname;
    }
}