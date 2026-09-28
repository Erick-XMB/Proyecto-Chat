namespace protocoloMensajes;

/// <summary>
/// Clase que hereda de Response que representa el mensaje UserAlreadyExits
/// </summary>
public class UserAlreadyExist : Response
{

    /// <summary>
    /// Constructor de la clase UserAlreadyExist
    /// </summary>
    /// <param name="extra"> es el nombre de usuario que ya esta registrado </param>
    public UserAlreadyExist(string extra)
    {
        operation = "IDENTIFY";
        result = "USER_ALREADY_EXISTS";
        this.extra = extra;
    }


}