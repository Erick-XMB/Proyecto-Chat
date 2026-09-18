namespace protocoloMensajes;

/// <summary>
/// Clase que hereda de mensaje que representa un mensaje de NewStatus
/// </summary>
public class NewStatus : Mensaje
{
    /// <summary>
    /// Nombre de; usuario
    /// </summary>
    public string username { get; set; }
    /// <summary>
    /// Estado del usuario
    /// </summary>
    /// <value></value>
    public string status { get; set; }

    /// <summary>
    /// Constructor del mensaje NewStatus
    /// </summary>
    /// <param name="username"> es el nombre del usuario </param>
    /// <param name="status"> es el estado del usuario </param>
    public NewStatus(string username, string status)
    {
        type = "NEW_STATUS";
        this.username = username;
        this.status = status;

    }


}