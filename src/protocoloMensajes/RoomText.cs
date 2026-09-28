namespace protocoloMensajes;

/// <summary>
/// Clase que hereda de mensaje que representa
/// la peticion de enviar un mensaje a un cuarto
/// </summary>
public class RoomText : Mensaje
{

    /// <summary>
    /// Atributo que representa el nombre del cuarto
    /// </summary>
    /// <value></value>
    public string roomname{get; set;}

    /// <summary>
    /// Atributo que representa texto del mensaje
    /// </summary>
    /// <value></value>
    public string text {get; set;}

    /// <summary>
    /// Constructor de la clase RoomText
    /// </summary>
    /// <param name="roomname"> es el nombre del cuarto </param>
    /// <param name="text"> es el texto del mensaje </param>
    public RoomText(string roomname, string text)
    {   
        type = "ROOM_TEXT";
        this.roomname = roomname;
        this.text = text;
    }    


}