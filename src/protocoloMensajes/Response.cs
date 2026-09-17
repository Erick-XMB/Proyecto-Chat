namespace protocoloMensajes;
public class Response : Mensaje
{

    public string operation { get; set; }
    public string result { get; set; }
    public string extra { get; set; }

    public Response()
    {
        this.type = "RESPONSE";
    }


}