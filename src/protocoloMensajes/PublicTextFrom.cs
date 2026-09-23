namespace protocoloMensajes;

/// <summary>
/// Clase que hereda de mensaje que representa la instruccion PublicTextFrom
/// </summary>
public class PublicTextFrom : Mensaje
{   
    /// <summary>
    /// Atributo que representa el nombre de usuario de quien envia el mensaje
    /// </summary>
    public string username{get; set;}

    /// <summary>
    /// Atributo que representa el mensaje que se envia
    /// </summary>
    public string text{get; set;}

    /// <summary>
    /// Constructor del mensaje Public text from
    /// </summary>
    /// <param name="username"> es el nombre de quien envua el mensaje </param>
    /// <param name="text"> es el mensaje que se esta enviando </param>
    public PublicTextFrom(string username, string text)
    {
        type = "PUBLIC_TEXT_FROM";
        this.username = username;
        this.text = text;
    }   
}