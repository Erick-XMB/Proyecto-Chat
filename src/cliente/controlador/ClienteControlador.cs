using System;
using ClienteChat.modelo;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClienteChat.controlador;

public class ClienteControlador{


    private readonly ConexionServidor cliente;

    /// <summary>
    /// Constructor de CLienteControlador
    /// </summary>
    public ClienteControlador(){
        cliente = new ConexionServidor();
    }

    /// <summary>
    /// Se solicita que establezca una conexion TCP
    /// </summary>
    /// <returns>
    /// <c>true</c> si la conexion se establezca correctamente
    /// <c>false</c> si hay un error
    /// </returns>
    public bool Conectar(int puerto){
        return cliente.Conectar(puerto);
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


    /** { "type": "IDENTIFY","username": "Kimberly" }*/
    public void Identificar(string username)
    {   
        Identify mensaje = new Identify(username);
        string json =  JsonSerializer.Serialize(mensaje);
        cliente.EnviarMensaje(json);
    }

    public async Task RecibirMensaje()
    {
       await cliente.RecibirMensajes();
    }

}