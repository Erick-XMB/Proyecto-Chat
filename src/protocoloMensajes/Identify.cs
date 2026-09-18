namespace protocoloMensajes;

/// <summary>
/// Clase que hereda de mensaje con la operacion Identify
/// </summary>
public class Identify : Mensaje
{   
    /// <summary>
    /// Nombre de usuario con el que el cliente desea identificarse.
    /// </summary>
    public string username { get; set; }

    /// <summary>
    /// Constructor de nuestro mensaje Identify
    /// </summary>
    /// <param name="username"> el nombre del usuario que le asignamos a la operacion </param>
    public Identify(string username)
    {

        type = "IDENTIFY";
        this.username = username;

    }


}