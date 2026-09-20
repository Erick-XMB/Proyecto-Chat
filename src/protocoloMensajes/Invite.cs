namespace protocoloMensajes;
/// <summary>
/// Clase que hereda de Mensaje que representa la invitacion a un cuarto
/// </summary>
public class Invite : Mensaje
{   
    /// <summary>
    /// Atribtuo que representa el nombre del cuarto
    /// </summary>
    public string roomname{get; set;}

    /// <summary>
    /// Lista que representa los nombres de usuarios que estan
    /// en el cuarto
    /// </summary>
    public List<string> usernames {get; set;}

    /// <summary>
    /// Constructor de la clase Invite
    /// </summary>
    /// <param name="roomname"> es el nombre del cuarto al que se invita </param>
    /// <param name="usernames"> es la lista de los nombres de usuario s</param>
    public Invite(string roomname, List<string> usernames)
    {
        type = "INVITE";
        this.roomname  =  roomname;
        this.usernames = usernames;
    }  

}