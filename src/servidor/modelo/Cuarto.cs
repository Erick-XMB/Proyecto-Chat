using System.Net;
using System.Net.Sockets;
using System.Text;
using ServidorChat.Modelo;

/// <summary>
/// Clase que representa los cuartos privados que tendra el chat
/// </summary>
public class Cuarto
{

    /// <summary>
    /// Atributo cadena que representa el nombre del cuarto
    /// </summary>
    private string roomname;

    /// <summary>
    /// Atributo Lista que representa a los usuarios en el cuarto
    /// </summary>
    private List<ConexionCliente> usuarios;

    /// <summary>
    /// Atributo que representa a las conexiones invitadas al cuarto
    /// </summary>
    private List<ConexionCliente> invitados;


    /// <summary>
    /// Constructor de la clase Cuarto
    /// </summary>
    /// <param name="roomname"> es el nombre del cuarto</param>
    public Cuarto(string roomname)
    {
        this.roomname = roomname;
        usuarios = new List<ConexionCliente>();
        invitados = new List<ConexionCliente>();
    }

    /// <summary>
    /// Metodo que devuelve el nombre del cuarto
    /// </summary>
    /// <returns> Devuelve el atributo con el nombre del cuarto </returns>
    public string GetRoomname()
    {
        return this.roomname;
    }

    /// <summary>
    /// Metodo que nos devuelve la lista de usuarios
    /// </summary>
    /// <returns> la lista de usuarios en el cuarto </returns>
    public List<ConexionCliente> GetUsuarios()
    {
        // una copia
        return new List<ConexionCliente>(usuarios);
    }

    /// <summary>
    /// Metodo que nos devuelve la lista de invitados
    /// </summary>
    /// <returns> la lista de invitados en el cuarto </returns>
    public List<ConexionCliente> GetInvitados()
    {   
        // una copia 
        return new List<ConexionCliente>(invitados);
    }

    /// <summary>
    /// Metodo que invita a una conexion al cuarto
    /// Verificamos si ya esta en el cuarto o si fue invitado
    /// </summary>
    /// <param name="cliente"> es la conexion a la que queremos invitar </param>
    /// <returns> false en caso de que ya este en el cuarto o ya haya sido invitado
    /// true en otro caso, y lo agregamos a la lista de invitados</returns>
    public bool Invitar(ConexionCliente cliente)
    {
        if (EstaEnElCuarto(cliente) || FueInvitado(cliente))
        {
            return false;
        }

        invitados.Add(cliente);
        return true;
    }

    /// <summary>
    /// Metodo que nos dice si una conexion ya fue invitada al cuarto
    /// </summary>
    /// <param name="cliente"> es la conexion que queremos verificar</param>
    /// <returns> true si ya esta en la lista de invitados, false en otro caso </returns>
    public bool FueInvitado(ConexionCliente cliente)
    {
        return invitados.Contains(cliente);
    }

    /// <summary>
    /// Metodo que nos dice si una conexion ya esta en el cuarto
    /// </summary>
    /// <param name="cliente"> Es la conexion que queremso verificar </param>
    /// <returns>true si ya esta en el cuarto, false en otro caso </returns>
    public bool EstaEnElCuarto(ConexionCliente cliente)
    {
        return usuarios.Contains(cliente);
    }

    /// <summary>
    /// Metddo que nos permite agregar a una conexion al cuarto
    /// </summary>
    /// <param name="cliente"> es la conexion que queremos agregar </param>
    public void AgregarUsuario(ConexionCliente cliente)
    {
        usuarios.Add(cliente);
    }

    /// <summary>
    /// Metodo que nos permite eliminar a un usuario del cuarto
    /// </summary>
    /// <param name="cliente"> es la conexion que queremos eliminar </param>
    public void EliminarUsuario(ConexionCliente cliente)
    {
        usuarios.Remove(cliente);
    }

    /// <summary>
    /// Metodo que nos permite eliminar a un usuario de los invitados
    /// </summary>
    /// <param name="cliente"> es la conexion que queremos eliminar </param>
    public void EliminarInvitado(ConexionCliente cliente)
    {
        invitados.Remove(cliente);
    }

    /// <summary>
    /// Metodo que nos devuelve si una sala esta vacia, contando
    /// si la lista de invitados es 0
    /// </summary>
    /// <returns> true si el numero en la lista de invitados es 0
    /// false en otro caso </returns>
    public bool EstaVacio()
    {
        if (invitados.Count == 0)
        {
            return true;
        }

        return false;
    }

}