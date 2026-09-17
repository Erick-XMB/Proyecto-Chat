public class NewStatus : Mensaje
{
    public string username { get; set; }
    public string status { get; set; }

    public NewStatus(string username, string status)
    {
        type = "NEW_STATUS";
        this.username = username;
        this.status = status;

    }


}