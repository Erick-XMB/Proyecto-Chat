namespace protocoloMensajes;
public class UserList : Mensaje
{
    public Dictionary<string, string> users { get; set; }

    public UserList(Dictionary<string, string> users)
    {
        type = "USER_LIST";
        this.users = users;
    }


}