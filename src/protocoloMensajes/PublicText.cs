//```
//{ "type": "PUBLIC_TEXT",
//  "text": "¡Hola a todos!" }
//```

namespace protocoloMensajes;

/// <summary>
/// Clase que hereda de Mensaje que representa el mensaje Public Text
/// </summary>
public class PublicText : Mensaje
{

    /// <summary>
    /// Atributo que representa el texto que queremos enviar
    /// </summary>
    public string text { get; set; }

    /// <summary>
    /// Constructor del mensaje PublicText
    /// </summary>
    /// <param name="text"> es el texto que queremos enviar</param>
    public PublicText(string text)
    {
        type = "PUBLIC_TEXT";
        this.text = text;
    }

}