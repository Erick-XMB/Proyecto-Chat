namespace protocoloMensajes;

/// <summary>
/// Clase que hereda de Response que representa 
/// el mensaje que alguien no fue previamente invitado
/// </summary>
public class NotInvited : Response
{   
    /// <summary>
    /// Clase que construye el mensaje de NoSuchRoom
    /// </summary>
    /// <param name="roomname"> es el nombre del cuarto </param>
    public NotInvited(string roomname)
    {
        operation = "JOIN_ROOM";
        result = "NOT_INVITED";
        extra =roomname;
    }




}