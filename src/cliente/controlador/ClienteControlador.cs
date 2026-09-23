using System;
using ClienteChat.modelo;
using System.Text.Json;
using System.Threading.Tasks;
using protocoloMensajes;
using System.Collections.Generic;

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

    public event Action<Mensaje>? MensajeParaInterfaz;

    /// <summary>
    /// Constructor de CLienteControlador
    /// </summary>
    public ClienteControlador()
    {
        cliente = new ConexionServidor();
        cliente.MensajeRecibido += MensajeRecibido;
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

    public async void PrivText(string username, string text)
    {
        PrivText privText = new PrivText(username, text);
        string jsonPrivText = JsonSerializer.Serialize(privText);
        cliente.EnviarMensaje(jsonPrivText);
    }

    /// <summary>
    /// Metodo que envia al serivdor la solicitud de PublicText
    /// </summary>
    /// <param name="text"> es el texto que queremos enviar </param>
    public async void PublicText(string text)
    {
        PublicText publicText = new PublicText(text);
        string jsonPublicText = JsonSerializer.Serialize(publicText);
        cliente.EnviarMensaje(jsonPublicText);
    }

    public async void NewRoom(string roomname)
    {
        NewRoom newRoom = new NewRoom(roomname);
        string jsonNewRoom = JsonSerializer.Serialize(newRoom);
        cliente.EnviarMensaje(jsonNewRoom);
    }

    public async void Invite(string roomname, List<string> usernames)
    {
        Invite invite = new Invite(roomname, usernames);
        string jsonInvite = JsonSerializer.Serialize(invite);
        cliente.EnviarMensaje(jsonInvite);
    }

    public async void JoinRoom(string roomname)
    {
        JoinRoom joinRoom = new JoinRoom(roomname);
        string jsonJoinRoom = JsonSerializer.Serialize(joinRoom);
        cliente.EnviarMensaje(jsonJoinRoom);
    }

    public async void RoomUsers(string roomname)
    {
        RoomUsers roomUsers = new RoomUsers(roomname);
        string jsonRoomUsers = JsonSerializer.Serialize(roomUsers);
        cliente.EnviarMensaje(jsonRoomUsers);
    }

    public async void RoomText(string roomname, string text)
    {
        RoomText roomText = new RoomText(roomname, text);
        string jsonRoomText = JsonSerializer.Serialize(roomText);
        cliente.EnviarMensaje(jsonRoomText);
    }

    public async void LeaveRoom(string roomname)
    {
        LeaveRoom leaveRoom = new LeaveRoom(roomname);
        string jsonLeaveRoom = JsonSerializer.Serialize(roomname);
        cliente.EnviarMensaje(jsonLeaveRoom);
    }

    public async void Disconnect()
    {
        Disconnect disconnect = new Disconnect();
        string jsonDisconnect = JsonSerializer.Serialize(disconnect);
        cliente.EnviarMensaje(jsonDisconnect);
    }

    public void MensajeRecibido(string mensaje)
    {
        Console.WriteLine(mensaje);

        Mensaje? mensajeBase = JsonSerializer.Deserialize<Mensaje>(mensaje);
        string tipoDeMensajeBase = mensajeBase.type;

        if (mensajeBase == null)
        {
            return;
        }

        switch (tipoDeMensajeBase)
        {
            case "NEW_USER":
                NewUser? newUser = JsonSerializer.Deserialize<NewUser>(mensaje);
                MensajeParaInterfaz?.Invoke(newUser);
                break;

            case "PUBLIC_TEXT_FROM":
                PublicTextFrom? PublicTextFrom = JsonSerializer.Deserialize<PublicTextFrom>(mensaje);
                MensajeParaInterfaz?.Invoke(PublicTextFrom);
                break;
            case "DISCONNECTED":
                Disconnected? disconnected = JsonSerializer.Deserialize<Disconnected>(mensaje);
                MensajeParaInterfaz?.Invoke(disconnected);
                break;

            case "NEW_STATUS":
                NewStatus? newStatus = JsonSerializer.Deserialize<NewStatus>(mensaje);
                MensajeParaInterfaz?.Invoke(newStatus);
                break;

            case "USER_LIST":
                UserList? userList = JsonSerializer.Deserialize<UserList>(mensaje);
                MensajeParaInterfaz?.Invoke(userList);
                break;

            case "TEXT_FROM":
                PrivTextFrom? privTextFrom = JsonSerializer.Deserialize<PrivTextFrom>(mensaje);
                MensajeParaInterfaz?.Invoke(privTextFrom);
                break;
        }
    }

    public async Task RecibirMensajes()
    {
        await cliente.RecibirMensajes();
    }



}