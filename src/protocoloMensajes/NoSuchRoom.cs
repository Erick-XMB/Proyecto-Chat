namespace protocoloMensajes;

/// <summary>
/// Clase que hereda de Response que representa
/// cuando no se pude encontrar la sala
/// </summary>
public class NoSuchRoom : Response
{
    /// <summary>
    /// Constructor vacio 
    /// </summary>
    public NoSuchRoom()
    {

    }

    /// <summary>
    /// Constrcuctor del mensaje NoSuhcRoom
    /// </summary>
    /// <param name="operation"> es la operacion que se hizo </param>
    /// <param name="roomname"> es el nombre del cuarto que ya no existe </param>
    public NoSuchRoom(string operation, string roomname)
    {
        this.operation = operation;
        result = "NO_SUCH_ROOM";
        extra = roomname;
    }



}