namespace protocoloMensajes;
/// <summary>
/// Clase que hereda de mensaje que representa la instruccion de crear un nuevo cuarto
/// </summary>
public class NewRoom : Mensaje
{
    /// <summary>
    /// Atributo que representa el nombre del nuevo cuarto
    /// </summary>
    public string roomname { get; set; }

    /// <summary>
    /// Contructor de la clase NewRoom
    /// </summary>
    /// <param name="roomname"> es el nombre con el que queremos identificar al cuarto</param>
    public NewRoom(string roomname)
    {
        type = "NEW_ROOM";
        this.roomname = roomname;
    }
}