namespace protocoloMensajes;
public class NewUser : Mensaje
{
    public string username{get; set;}

    public NewUser(string username)
    {
        type = "NEW_USER";
        this.username =  username;
    }
}