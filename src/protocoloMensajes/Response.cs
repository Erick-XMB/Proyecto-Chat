namespace protocoloMensajes;

/// <summary>
/// Clase que hereda de Mensaje que representa el tipo de mensaje Response
/// </summary>
public class Response : Mensaje
{
    /// <summary>
    /// Atributo que representa el tipo de operacion
    /// </summary>
    public string operation { get; set; }

    /// <summary>
    /// Atributo que representa el tipo de resultado
    /// </summary>
    public string result { get; set; }

    /// <summary>
    /// Atributo que representa informacion extra
    /// </summary>
    /// <value></value>
    public string extra {get; set;}

    /// <summary>
    /// Constructor de la clase Response
    /// </summary>
    public Response()
    {
        this.type = "RESPONSE";
    }


}