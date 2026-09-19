namespace protocoloMensajes;


/// <summary>
/// Clase que hereda de response que representa cuando
/// no se encontro un usuario
/// </summary>
public class NoSuchUser : Response
{      

    /// <summary>
    /// Constructor del mensaje NoSuchUser
    /// </summary>
    /// <param name="extra"></param>
    public NoSuchUser(string extra)
    {
        this.operation = "TEXT";
        this.result = "NO_SUCH_USER";
        this.extra = extra;
    }



}