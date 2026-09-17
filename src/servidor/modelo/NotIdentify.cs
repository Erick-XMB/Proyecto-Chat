/*   ```
{ "type": "RESPONSE",
  "operation": "INVALID",
  "result": "NOT_IDENTIFIED" }
  ```*/
public class NotIdentify : Response
{
    public string operation { get; set; }

    public string result { get; set; }

    public NotIdentify()
    {
        this.operation = "INVALID";
        this.result = "NOT_IDENTIFIED";
    }


}