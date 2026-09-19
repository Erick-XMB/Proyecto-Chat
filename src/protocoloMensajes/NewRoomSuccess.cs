namespace protocoloMensajes;

/// <summary>
/// Clase que hereda de response que representa que se pudo
/// crear de manera exitosa un cuarto
/// </summary>
public class NewRoomSucess : Response
{
    /// <summary>
    /// Contructor de la clase NewRoomSucess
    /// </summary>
    /// <param name="roomname"></param>
    public NewRoomSucess(string roomname)
    {
        operation = "NEW_ROOM";
        result = "SUCCESS";
        extra = roomname;
    }


}