namespace protocoloMensajes;
/// <summary>
/// Clase que hereda de mensaje que representa el mensaje de UserList
/// </summary>
public class UserList : Mensaje
{   
    /// <summary>
    /// Atributo de diccionario que tiene cadenas
    /// </summary>
    /// <value></value>
    public Dictionary<string, string> users { get; set; }

    /// <summary>
    /// Constructo de la clase UserList
    /// </summary>
    /// <param name="users"> es el diccionario de usuarios </param>
    public UserList(Dictionary<string, string> users)
    {
        type = "USER_LIST";
        this.users = users;
    }


}