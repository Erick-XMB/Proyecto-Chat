using System;
using ClienteChat.modelo;
using System.Text.Json;
using System.Threading.Tasks;
using protocoloMensajes;

namespace ClienteChat.controlador;

/// <summary>
/// Clase que representa el controlador de nuestro cliente
/// </summary>
public class ClienteControlador
{


    /// <summary>
    /// Atributo que representa la conexion
    /// </summary>
    private readonly ConexionServidor cliente;

    /// <summary>
    /// Constructor de CLienteControlador
    /// </summary>
    public ClienteControlador()
    {
        cliente = new ConexionServidor();
    }

    /// <summary>
    /// Se solicita que establezca una conexion TCP
    /// </summary>
    /// <returns>
    /// <c>true</c> si la conexion se establezca correctamente
    /// <c>false</c> si hay un error
    /// </returns>
    public bool Conectar(int puerto)
    {
        return cliente.Conectar(puerto);
    }

    /// <summary>
    /// Envia un mensaje al servidor mediante la conexion 
    /// </summary>
    /// <param name="mensaje">
    /// Cadena de texto que se quiere enviar al servidor
    /// </param>
    public void EnviarMensaje(string mensaje)
    {
        cliente.EnviarMensaje(mensaje);
    }


    /// <summary>
    /// Metodo que envia al servidor la solicitud de identificarse
    /// </summary>
    /// <param name="username">
    /// nombre de usuario con el que el cliente desea identificarse</param>
    /// <returns> true si se identifico </returns>
    public bool Identificar(string username)
    {
        Identify mensaje = new Identify(username);
        string json = JsonSerializer.Serialize(mensaje);
        cliente.EnviarMensaje(json);
        return true;
    }

    /// <summary>
    /// Metodo que envia al servidor la solicitud de status
    /// </summary>
    /// <param name="status">
    ///  Es el status que queremos enviar</param>
    public void Status(string status)
    {
        Status statusUsuaio = new Status(status);
        string json = JsonSerializer.Serialize(statusUsuaio);

        cliente.EnviarMensaje(json);

    }

    /// <summary>
    /// Metodo que envia al servidor la solicitud de users
    /// </summary>
    public void Users()
    {
        Users users = new Users();
        string json = JsonSerializer.Serialize(users);

        cliente.EnviarMensaje(json);
    }

    /// <summary>
    /// Metodo que envia al serivdor la solicitud de PublicText
    /// </summary>
    /// <param name="text"> es el texto que queremos enviar </param>
    public async void PublicText(string text)
    {
        PublicText publicText = new PublicText(text);
        string jsonPublicText =  JsonSerializer.Serialize(publicText);
        cliente.EnviarMensaje(jsonPublicText);
    }

    /// <summary>
    /// Metodo que recibe mensajes que le llegan al cliente
    /// </summary>
    /// <returns> Una tarea que reprsenta la operacion de recibir mensajes</returns>
    public async Task RecibirMensaje()
    {
        await cliente.RecibirMensajes();
    }

}