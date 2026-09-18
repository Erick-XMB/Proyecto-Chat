namespace protocoloMensajes;
/// <summary>
/// Clase que hereda de Mensaje que representa el mensaje de Users
/// </summary>  
public class Users : Mensaje
{   
    /// <summary>
    /// Constrctor del mensaje users
    /// </summary>
    public Users()
    {
        type = "USERS";
    }
}