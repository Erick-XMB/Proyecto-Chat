namespace protocoloMensajes;
/// <summary>
/// Clase que hereda de Mensaje que representa que se te invito a un cuarto
/// </summary>
public class Invitation : Mensaje
{
    /// <summary>
    /// Es el nombre del usuario que recibe la invitacion
    /// </summary>
    public string username { get; set; }

    /// <summary>
    /// Atribtuo que representa el nombre del cuarto
    /// </summary>
    public string roomname { get; set; }


    /// <summary>
    /// Constructor de la clase Invitation
    /// </summary>
    /// <param name="roomname"> es el nombre del cuarto al que se invita </param>
    /// <param name="username"> es el nombre del usuario que recibe la invitacion </param>
    public Invitation(string username, string roomname)
    {
        type = "INVITATION";
        this.username = username;
        this.roomname = roomname;
    }

}