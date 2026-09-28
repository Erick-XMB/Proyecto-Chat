namespace protocoloMensajes;

/// <summary>
/// Clase que hereda de Mensaje que representa 
/// la lista de usuarios de una sala
/// </summary>
public class RoomUserList : Mensaje
{   
    /// <summary>
    /// Atributo que representa el nombre de la sala
    /// </summary>
    /// <value></value>
    public string roomname { get; set; }

    /// <summary>
    /// Diccionario que representa el nombre de usuario y su estado
    /// </summary>
    /// <value></value>
    public Dictionary<string, string> users { get; set; }

    /// <summary>
    /// Constructor de la clase RoomUserList
    /// </summary>
    /// <param name="roomname"> es el nombre de la sala </param>
    /// <param name="users"> Diccionario con usuarios y sus estados</param>
    public RoomUserList(string roomname, Dictionary<string, string> users)
    {   
        type = "ROOM_USER_LIST";
        this.roomname = roomname;
        this.users = users;
    }



}