namespace protocoloMensajes;

/// <summary>
/// Clase que hereda de Response que representa que un cuarto
/// ya existe con el nombre propuesto
/// </summary>
public class RoomAlreadyExists : Response
{

    /// <summary>
    /// Constructor de la clase RoomAlreadyExists
    /// </summary>
    /// <param name="roomname"></param>
    public RoomAlreadyExists(string roomname)
    {
        operation = "NEW_ROOM";
        result = "ROOM_ALREADY_EXISTS";
        extra = roomname;   
    }



}