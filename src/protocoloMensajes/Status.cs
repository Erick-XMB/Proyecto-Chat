namespace protocoloMensajes;
public class Status : Mensaje
{
  /** { "type": "NEW_STATUS",
"username": "Kimberly",
"status": "AWAY" }*/

  public string status { get; set; }

  public Status(string status)
  {
    type = "STATUS";
    this.status = status;

  }


}