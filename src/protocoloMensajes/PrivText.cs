namespace protocoloMensajes;

/// <summary>
/// Clase que hereda de mensaje que representa enviar un texto privado 
/// </summary>
public class PrivText : Mensaje
{   

    /// <summary>
    /// Atribbuto que representa el usuario a quien se manda el mensaje
    /// </summary>
    public string username { get; set; }

    /// <summary>
    /// Atributo que representa la cadena del mensaje
    /// </summary>
    public string text { get; set; }

    /// <summary>
    /// Csontructor de la clase Text
    /// </summary>
    /// <param name="username"> es el nombre de quien recibira el mensaje</param>
    /// <param name="text"> es la cadena de texto que representa el mensaje</param>
    public PrivText(string username, string text)
    {   
        type = "TEXT";
        this.username =  username;
        this.text = text;
    }
}