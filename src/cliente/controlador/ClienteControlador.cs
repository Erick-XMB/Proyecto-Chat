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

    /// <summary>
    /// Evento que nos dispara una accion Mensaje
    /// </summary>
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

    public void Desconectar()
    {
        cliente.Desconectar();
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
    public void Identificar(string username)
    {
        Identify mensaje = new Identify(username);
        string json = JsonSerializer.Serialize(mensaje);
        cliente.EnviarMensaje(json);
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
    /// Metodo que envia al servidor la solicitud de un texto privado
    /// </summary>
    /// <param name="username"> es a quien queremos enviar el texto privado</param>
    /// <param name="text"> es el texto </param>
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

    /// <summary>
    /// Metodo que envia al servidor la solicitud de crear un nuevo sala
    /// </summary>
    /// <param name="roomname"> es el nombre con el que queremos crear el sala </param>
    public async void NewRoom(string roomname)
    {
        NewRoom newRoom = new NewRoom(roomname);
        string jsonNewRoom = JsonSerializer.Serialize(newRoom);
        cliente.EnviarMensaje(jsonNewRoom);
    }

    /// <summary>
    /// Metodo que envia al servidor la solicitud de invitar a alguien a un sala
    /// </summary>
    /// <param name="roomname"> es el nombre del sala </param>
    /// <param name="usernames"> es la lista de personas a quienes queremos invitar </param>
    public async void Invite(string roomname, List<string> usernames)
    {
        Invite invite = new Invite(roomname, usernames);
        string jsonInvite = JsonSerializer.Serialize(invite);
        cliente.EnviarMensaje(jsonInvite);
    }

    /// <summary>
    /// Metodo que envia al servidor la solicitud de Unirse a un sala
    /// </summary>
    /// <param name="roomname"> es el sala al que nos queremos unir </param>
    public async void JoinRoom(string roomname)
    {
        JoinRoom joinRoom = new JoinRoom(roomname);
        string jsonJoinRoom = JsonSerializer.Serialize(joinRoom);
        cliente.EnviarMensaje(jsonJoinRoom);
    }

    /// <summary>
    /// Metodo que envia al servidor la solictud de mostrar los usuarios de un sala
    /// </summary>
    /// <param name="roomname"> es el nombre del sala al que nos queremso unir </param>
    public async void RoomUsers(string roomname)
    {
        RoomUsers roomUsers = new RoomUsers(roomname);
        string jsonRoomUsers = JsonSerializer.Serialize(roomUsers);
        cliente.EnviarMensaje(jsonRoomUsers);
    }

    /// <summary>
    /// Metodo que envia al servidor la solicitud de mandar un texto a una sala
    /// </summary>
    /// <param name="roomname"> es el nombre de la sala </param>
    /// <param name="text"> es el texto que queremos enviar </param>
    public async void RoomText(string roomname, string text)
    {
        RoomText roomText = new RoomText(roomname, text);
        string jsonRoomText = JsonSerializer.Serialize(roomText);
        cliente.EnviarMensaje(jsonRoomText);
    }

    /// <summary>
    /// Metodo que envia al servidor la solictud de abandonar un cuarto
    /// </summary>
    /// <param name="roomname"> es el nombre de la sala </param>
    public async void LeaveRoom(string roomname)
    {
        LeaveRoom leaveRoom = new LeaveRoom(roomname);
        string jsonLeaveRoom = JsonSerializer.Serialize(leaveRoom);
        cliente.EnviarMensaje(jsonLeaveRoom);
    }

    /// <summary>
    /// Metodo que envia la solicitud al servidor de desconectarse
    /// </summary>
    public async void Disconnect()
    {
        Disconnect disconnect = new Disconnect();
        string jsonDisconnect = JsonSerializer.Serialize(disconnect);
        cliente.EnviarMensaje(jsonDisconnect);
    }

    /// <summary>
    /// Metodo que nos permite enviar invocar nuestro evento de MensajeParaInterfaz con el tipo de mensaje que nos llego 
    /// veificamos que el mensaje no sea vacio, si no lo e, obtenermos el tipo del mensaje
    /// y dado eso es que manejamos los objetos de los mensajes
    /// </summary>
    /// <param name="mensaje"></param>
    public void MensajeRecibido(string mensaje)
    {
        if (string.IsNullOrWhiteSpace(mensaje))
        {
            return;
        }

        try
        {
            Console.WriteLine(mensaje);

            Mensaje? mensajeBase = JsonSerializer.Deserialize<Mensaje>(mensaje);

            if (mensajeBase == null)
            {
                return;
            }

            string tipoDeMensajeBase = mensajeBase.type;

            switch (tipoDeMensajeBase)
            {
                case "NEW_USER":
                    NewUser? newUser = JsonSerializer.Deserialize<NewUser>(mensaje);
                    if (newUser != null)
                    {
                        MensajeParaInterfaz?.Invoke(newUser);
                    }
                    break;

                case "INVITATION":
                    Invitation? invitation = JsonSerializer.Deserialize<Invitation>(mensaje);
                    if (invitation != null)
                    {
                        MensajeParaInterfaz?.Invoke(invitation);
                    }
                    break;

                case "JOINED_ROOM":
                    JoinedRoom? joinedRoom = JsonSerializer.Deserialize<JoinedRoom>(mensaje);
                    if (joinedRoom != null)
                    {
                        MensajeParaInterfaz?.Invoke(joinedRoom);
                    }
                    break;


                case "PUBLIC_TEXT_FROM":
                    PublicTextFrom? PublicTextFrom = JsonSerializer.Deserialize<PublicTextFrom>(mensaje);
                    if (PublicTextFrom != null)
                    {
                        MensajeParaInterfaz?.Invoke(PublicTextFrom);
                    }
                    break;

                case "ROOM_USER_LIST":
                    RoomUserList? roomUserList = JsonSerializer.Deserialize<RoomUserList>(mensaje);
                    if (roomUserList != null)
                    {
                        MensajeParaInterfaz?.Invoke(roomUserList);
                    }
                    break;

                case "LEFT_ROOM":
                    LeftRoom? leftRoom = JsonSerializer.Deserialize<LeftRoom>(mensaje);
                    if (leftRoom != null)
                    {
                        MensajeParaInterfaz?.Invoke(leftRoom);
                    }
                    break;

                case "DISCONNECTED":
                    Disconnected? disconnected = JsonSerializer.Deserialize<Disconnected>(mensaje);
                    if (disconnected != null)
                    {
                        MensajeParaInterfaz?.Invoke(disconnected);
                    }
                    break;

                case "NEW_STATUS":
                    NewStatus? newStatus = JsonSerializer.Deserialize<NewStatus>(mensaje);
                    if (newStatus != null)
                    {
                        MensajeParaInterfaz?.Invoke(newStatus);
                    }
                    break;

                case "USER_LIST":
                    UserList? userList = JsonSerializer.Deserialize<UserList>(mensaje);
                    if (userList != null)
                    {
                        MensajeParaInterfaz?.Invoke(userList);
                    }
                    break;

                case "ROOM_TEXT_FROM":
                    RoomTextFrom? roomTextFrom = JsonSerializer.Deserialize<RoomTextFrom>(mensaje);
                    if (roomTextFrom != null)
                    {
                        MensajeParaInterfaz?.Invoke(roomTextFrom);
                    }
                    break;

                case "TEXT_FROM":
                    PrivTextFrom? privTextFrom = JsonSerializer.Deserialize<PrivTextFrom>(mensaje);
                    if (privTextFrom != null)
                    {
                        MensajeParaInterfaz?.Invoke(privTextFrom);
                    }
                    break;
                case "RESPONSE":
                    Response? response = JsonSerializer.Deserialize<Response>(mensaje);

                    if(response == null)
                    {
                        break;
                    }

                    string resultadoResponse = response.result;
                    string operacionResponse = response.operation;
                    

                    switch (resultadoResponse)
                    {
                        case "NO_SUCH_USER" when operacionResponse == "TEXT":
                            NoSuchUser? noSuchUser = JsonSerializer.Deserialize<NoSuchUser>(mensaje);
                            if (noSuchUser != null)
                            {
                                MensajeParaInterfaz?.Invoke(noSuchUser);
                            }
                            break;
                        case "NO_SUCH_USER" when operacionResponse == "INVITE":
                            NoSuchUser? noSuchUserInvite = JsonSerializer.Deserialize<NoSuchUser>(mensaje);
                            if (noSuchUserInvite != null)
                            {
                                MensajeParaInterfaz?.Invoke(noSuchUserInvite);
                            }
                            break;

                        case "NO_SUCH_ROOM" when operacionResponse == "INVITE":
                            NoSuchRoom? noSuchRoom = JsonSerializer.Deserialize<NoSuchRoom>(mensaje);
                            if (noSuchRoom != null)
                            {
                                MensajeParaInterfaz?.Invoke(noSuchRoom);
                            }
                            break;

                        case "NO_SUCH_ROOM" when operacionResponse == "JOIN_ROOM":
                            NoSuchRoom? noSuchRoomJoin = JsonSerializer.Deserialize<NoSuchRoom>(mensaje);
                            if (noSuchRoomJoin != null)
                            {
                                MensajeParaInterfaz?.Invoke(noSuchRoomJoin);
                            }
                            break;

                        case "NO_SUCH_ROOM" when operacionResponse == "LEAVE_ROOM":
                            NoSuchRoom? noSuchRoomLeave = JsonSerializer.Deserialize<NoSuchRoom>(mensaje);
                            if (noSuchRoomLeave != null)
                            {
                                MensajeParaInterfaz?.Invoke(noSuchRoomLeave);
                            }
                            break;

                        case "NO_SUCH_ROOM" when operacionResponse == "ROOM_USERS":
                            NoSuchRoom? noSuchRoomUsers = JsonSerializer.Deserialize<NoSuchRoom>(mensaje);
                            if (noSuchRoomUsers != null)
                            {
                                MensajeParaInterfaz?.Invoke(noSuchRoomUsers);
                            }
                            break;

                        case "NO_SUCH_ROOM" when operacionResponse == "ROOM_TEXT":
                            NoSuchRoom? noSuchRoomText = JsonSerializer.Deserialize<NoSuchRoom>(mensaje);
                            if (noSuchRoomText != null)
                            {
                                MensajeParaInterfaz?.Invoke(noSuchRoomText);
                            }
                            break;

                        case "NOT_INVITED" when operacionResponse == "ROOM_USERS":
                            NotInvited? notInvited = JsonSerializer.Deserialize<NotInvited>(mensaje);
                            if (notInvited != null)
                            {
                                MensajeParaInterfaz?.Invoke(notInvited);
                            }
                            break;

                        case "NOT_INVITED" when operacionResponse == "LEAVE_ROOM":
                            NotInvited? notInvitedLeave = JsonSerializer.Deserialize<NotInvited>(mensaje);
                            if (notInvitedLeave != null)
                            {
                                MensajeParaInterfaz?.Invoke(notInvitedLeave);
                            }
                            break;

                        case "NOT_JOINED" when operacionResponse == "ROOM_USERS":
                            NotJoined? notJoined = JsonSerializer.Deserialize<NotJoined>(mensaje);
                            if (notJoined != null)
                            {
                                MensajeParaInterfaz?.Invoke(notJoined);
                            }
                            break;

                        case "NOT_JOINED" when operacionResponse == "ROOM_TEXT":
                            NotJoined? notJoinedText = JsonSerializer.Deserialize<NotJoined>(mensaje);
                            if (notJoinedText != null)
                            {
                                MensajeParaInterfaz?.Invoke(notJoinedText);
                            }
                            break;

                        case "USER_ALREADY_EXISTS":
                            UserAlreadyExist? userAlreadyExist = JsonSerializer.Deserialize<UserAlreadyExist>(mensaje);
                            if (userAlreadyExist != null)
                            {
                                MensajeParaInterfaz?.Invoke(userAlreadyExist);
                            }
                            break;
                        case "NOT_IDENTIFIED":
                            NotIdentify? notIdentify = JsonSerializer.Deserialize<NotIdentify>(mensaje);
                            if (notIdentify != null)
                            {
                                MensajeParaInterfaz?.Invoke(notIdentify);
                            }
                            break;
                        case "SUCCESS" when operacionResponse == "IDENTIFY":
                            IdentifySuccess? identifySuccess = JsonSerializer.Deserialize<IdentifySuccess>(mensaje);
                            if (identifySuccess != null)
                            {
                                MensajeParaInterfaz?.Invoke(identifySuccess);
                            }
                            break;
                        case "INVALID":
                            Invalid? invalid = JsonSerializer.Deserialize<Invalid>(mensaje);
                            if (invalid != null)
                            {
                                MensajeParaInterfaz?.Invoke(invalid);
                            }
                            break;
                        case "SUCCESS" when operacionResponse == "NEW_ROOM":
                            NewRoomSucess? newRoomSucess = JsonSerializer.Deserialize<NewRoomSucess>(mensaje);
                            if (newRoomSucess != null)
                            {
                                MensajeParaInterfaz?.Invoke(newRoomSucess);
                            }
                            break;
                        case "ROOM_ALREADY_EXISTS":
                            RoomAlreadyExists? roomAlreadyExists = JsonSerializer.Deserialize<RoomAlreadyExists>(mensaje);
                            if (roomAlreadyExists != null)
                            {
                                MensajeParaInterfaz?.Invoke(roomAlreadyExists);
                            }
                            break;
                        case "SUCCESS" when operacionResponse == "JOIN_ROOM":
                            JoinRoomSuccess? joinRoomSuccess = JsonSerializer.Deserialize<JoinRoomSuccess>(mensaje);
                            if (joinRoomSuccess != null)
                            {
                                MensajeParaInterfaz?.Invoke(joinRoomSuccess);
                            }
                            break;
                    }
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    /// <summary>
    /// Tarea asincrona que nos permite recibir mensajes
    /// </summary>
    public async Task RecibirMensajes()
    {
        await cliente.RecibirMensajes();
    }



}