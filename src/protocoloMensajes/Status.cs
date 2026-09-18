namespace protocoloMensajes;  
/// <summary>
/// Clase que hereda de mensaje que representa la oepracion Status
/// </summary>
public class Status : Mensaje
{
  /// <summary>
  /// Atributo que representa el estado 
  /// </summary>
  public string status { get; set; }

  /// <summary>
  /// Cosntructor de la clase status
  /// </summary>
  /// <param name="status"> es el estado que queremos asignar en el mensaje</param>
  public Status(string status)
  {
    type = "STATUS";
    this.status = status;

  }


}