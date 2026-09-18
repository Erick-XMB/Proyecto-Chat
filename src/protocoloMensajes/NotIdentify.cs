
namespace protocoloMensajes;

/// <summary>
/// Clase que hereda de Response que representa el mensaje notIdentify
/// </summary>
public class NotIdentify : Response
{
    /// <summary>
    /// Constructor de nuestro mensaje NotIdentify
    /// </summary>
    public NotIdentify()
    {
        this.operation = "INVALID";
        this.result = "NOT_IDENTIFIED";
    }


}