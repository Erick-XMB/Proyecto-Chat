namespace protocoloMensajes;

/// <summary>
/// Clase que hereda de Response que representa 
/// Que una identificacion fue exitosa
/// </summary>
public class IdentifySuccess : Response
{

    public IdentifySuccess()
    {
    }
    /// <summary>
    /// Constructor de la clase JoinRoomSucces
    /// </summary>
    /// <param name="username"> es el nombre del usuario </param>
    public IdentifySuccess(string username)
    {
        operation = "IDENTIFY";
        result = "SUCCESS";
        extra = username;
    }


}