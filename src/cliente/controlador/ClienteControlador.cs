using Cliente.modelo;

namespace Cliente.controlador;

public class ClienteControlador{

    // Este es un dato que no se permite cambiar en tiempo de ejecucion
    private readonly ConexionCliente cliente;

    /// <summary>
    /// Constructor de CLienteControlador
    /// </summary>
    public ClienteControlador(){
        cliente = new ConexionCliente();
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