public class Status : Mensaje
{
    /** { "type": "NEW_STATUS",
  "username": "Kimberly",
  "status": "AWAY" }*/

    public string status;

    public Status(string status)
    {
       type = "STATUS";
       this.status = status;
    
    }


}