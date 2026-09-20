namespace protocoloMensajes;

/// <summary>
/// Clase que hereda de Response que representa 
/// Que unirse a un cuarto fue exitoso
/// </summary>
public class JoinRoomSuccess : Response
{
    /// <summary>
    /// Constructor de la clase JoinRoomSucces
    /// </summary>
    /// <param name="roomname"> es el nombre de la sala </param>
    public JoinRoomSuccess(string roomname)
    {
        operation = "JOIN_ROOM";
        result = "SUCCESS";
        extra = roomname;
    }


}