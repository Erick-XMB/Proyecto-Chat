using ClienteChat.modelo;

namespace ClienteChat.controlador;

public class ClienteControlador{


    private readonly ConexionCl cliente;

    /// <summary>
    /// Constructor de CLienteControlador
    /// </summary>
    public ClienteControlador(){
        cliente = new ConexionCl();
    }

    /// <summary>
    /// Se solicita que establezca una conexion TCP
    /// </summary>
    /// <returns>
    /// <c>true</c> si la conexion se establezca correctamente
    /// <c>false</c> si hay un error
    /// </returns>
    public bool Conectar(){
        return cliente.Conectar();
    }

    /// <summary>
    /// Envia un mensaje al servidor mediante la conexion 
    /// </summary>
    /// <param name="mensaje">
    /// Cadena de texto que se quiere enviar al servidor
    /// </param>
    public void EnviarMensaje(string mensaje){
        cliente.EnviarMensaje(mensaje);
    }



}