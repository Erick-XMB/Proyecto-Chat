namespace protocoloMensajes;

/// <summary>
/// Clase que hereda de mensaje que representa un texto privado
/// </summary>
public class PrivTextFrom :  Mensaje
{   
    /// <summary>
    /// Atributo que representa quien mando un mensaje
    /// </summary>
    public string username{get; set;}

    /// <summary>
    /// Cadena de texto que representa el texto
    /// </summary>
    public string text{get; set;}

    /// <summary>
    /// Constructor de la clase PrivTextFrom 
    /// </summary>
    /// <param name="username"> es el nombre de quien manda el mensaje</param>
    /// <param name="text"> es la cadena de texto que representa el mensaje</param>
    public PrivTextFrom(string username, string text)
    {
        type = "TEXT_FROM";
        this.username = username;
        this.text = text;
    }


}