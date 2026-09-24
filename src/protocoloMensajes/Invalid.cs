namespace protocoloMensajes;

/// <summary>
/// Clase que hereda de Response que representa 
/// una operacion invalida
/// </summary>
public class Invalid : Response
{
    /// <summary>
    /// Constructor de la clase JoinRoomSucces
    /// </summary>
    public Invalid()
    {
        operation = "INVALID";
        result = "INVALID";
    }


}