namespace protocoloMensajes;


/// <summary>
/// Clase que hereda de Response que representa el mensaje
/// Not Joined
/// </summary>
public class NotJoined : Response
{
    /// <summary>
    /// Constructor del mensaje NotJoined
    /// </summary>
    /// <param name="operation"> es el nombre de la operacion </param>
    /// <param name="roomname"> es el nombre de la sala </param>
    public NotJoined(string operation, string roomname)
    {
        this.operation = operation;
        result = "NOT_JOINED";
        extra = roomname;
    }
}