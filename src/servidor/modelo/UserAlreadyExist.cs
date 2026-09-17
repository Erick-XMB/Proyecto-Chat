public class UserAlreadyExist : Response
{
    /** { "type": "RESPONSE",
  "operation": "IDENTIFY",
  "result": "USER_ALREADY_EXISTS",
  "extra": "Kimberly" }*/
    public string operation { get; set; }

    public string result { get; set; }

    public string extra { get; set; }

    public UserAlreadyExist(string extra)
    {
        operation = "IDENTIFY";
        result = "USER_ALREADY_EXISTS";
        this.extra = extra;
    }


}