namespace protocoloMensajes;
/// <summary>
/// Clase que hereda de mensaje que representa un mensaje de NewUser
/// </summary>
public class NewUser : Mensaje
{
    /// <summary>
    /// Atributo que representa el nombre de usuario
    /// </summary>

    public string username{get; set;}

    /// <summary>
    /// Constructor de nuestrp mensaje NewUser
    /// </summary>
    /// <param name="username"> es el nombre de usuario </param>
    public NewUser(string username)
    {
        type = "NEW_USER";
        this.username =  username;
    }
}