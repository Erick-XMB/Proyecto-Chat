public class Identify : Mensaje
{
    public string username { get; set; }

    public Identify(string username)
    {

        type = "IDENTIFY";
        this.username = username;

    }


}