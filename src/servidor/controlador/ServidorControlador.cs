using ServidorChat.Modelo;
using System.Text.Json;
using protocoloMensajes;

namespace ServidorChat.controlador;
/// <summary>
/// Clase que representa el controlador de nuestro servidor
/// </summary>
public class ServidorControlador
{
    /// <summary>
    /// Atributo servidor que representa a un Servidor
    /// </summary>
    private readonly Servidor servidor;

    /// <summary>
    /// Lista clientes que es donde se guardan las conexiones de clientes que hagamos
    /// </summary>    
    private readonly List<ConexionCliente> clientes;

    private readonly Dictionary<string, Cuarto> cuartos = new Dictionary<string, Cuarto>();

    /// <summary>
    /// Constructor de nuestro ServidorControlador, donde iniciamos el servidor
    /// la lista con las conexiones y hcemos la suscripcion de una nueva conexion con su metodos
    /// asignado
    /// </summary>
    /// <param name="puerto"> el puerto del servido. </param>
    public ServidorControlador(int puerto)
    {
        servidor = new Servidor(puerto);
        clientes = new List<ConexionCliente>();

        servidor.nuevaConexion += NuevaConexion;
    }


    /// <summary>
    /// Metodo donde obtenemos el atributo servidor de nuestra clase
    /// </summary>
    /// <returns> el atributo servidor </returns>
    public Servidor GetServidor()
    {
        return this.servidor;
    }


    /// <summary>
    /// Metodo asincrono que maneja las nuevas conexiones que se hagan al servidor
    /// </summary>
    /// <param name="cliente"></param>
    /// <returns></returns>
    public async void NuevaConexion(ConexionCliente cliente)
    {
        /** Agregamos la conexion a nuestra lista*/
        clientes.Add(cliente);

        /** suscribimos el metodo MensajeRecibido al evento de mensajeRecibido*/
        cliente.mensajeRecibido += MensajeRecibido;

        /** esperamos a que el cliente reciba mensajes*/
        await cliente.RecibirMensajes();
    }

    /// <summary>
    /// Metodo que nos permite sacar de todos los cuartos que pertenezca una conexion
    /// Ademas si el cuarto queda vacio, este se elimina del diccionario de cuartos
    /// </summary>
    /// <param name="cliente"> es la conexion que queremos sacar de los cuartos </param>
    /// <returns> Una tarea que representa la operacion asincrona de Sacar de todos los cuartos a un cliente </returns>
    public async Task SacarDeTodosLosCuartos(ConexionCliente cliente)
    {
        string username = cliente.GetUsername();

        Dictionary<string, List<ConexionCliente>> usuariosEnElCuarto = new Dictionary<string, List<ConexionCliente>>();

        lock (cuartos)
        {
            List<KeyValuePair<string, Cuarto>> copiaDeLosCuartos = new List<KeyValuePair<string, Cuarto>>(cuartos);

            foreach (KeyValuePair<string, Cuarto> dupla in copiaDeLosCuartos)
            {
                Cuarto cuarto = dupla.Value;

                cuarto.EliminarInvitado(cliente);

                if (cuarto.EstaEnElCuarto(cliente))
                {
                    cuarto.EliminarUsuario(cliente);
                    usuariosEnElCuarto.Add(dupla.Key, cuarto.GetUsuarios());

                    if (cuarto.EstaVacio())
                    {
                        cuartos.Remove(dupla.Key);
                    }
                }
            }
        }

        foreach (KeyValuePair<string, List<ConexionCliente>> dupla in usuariosEnElCuarto)
        {
            LeftRoom leftRoom = new LeftRoom(dupla.Key, username);
            string jsonLeftRoom = JsonSerializer.Serialize(leftRoom);

            foreach (ConexionCliente c in dupla.Value)
            {
                await c.EnviarMensaje(jsonLeftRoom);
            }
        }
    }

    /// <summary>
    /// Metodo que nos permite cerrar la conexion de un cliente
    /// lo quitamos de la lista de clientes, lo sacamos de todos los cuartos en los que este
    /// y mandamos aviso a los demas usuarios
    /// </summary>
    /// <param name="cliente"> es la conexion del cliente que queremos cerrar </param>
    /// <returns> Una tarea que representa la operacion asincrona de sacar de todos los cuartos a un cliente</returns>
    public async Task CerrarConexion(ConexionCliente cliente)
    {
        bool estabaConectado;

        lock (clientes)
        {
            estabaConectado = clientes.Remove(cliente);
        }

        if (!estabaConectado)
        {
            return;
        }

        string username = cliente.GetUsername();

        if (username != null)
        {
            Disconnected disconnected = new Disconnected(username);
            string jsonDisconnected = JsonSerializer.Serialize(disconnected);

            List<ConexionCliente> copiaDeLosClientes;

            lock (clientes)
            {
                copiaDeLosClientes = new List<ConexionCliente>(clientes);
            }

            foreach (ConexionCliente c in copiaDeLosClientes)
            {
                await c.EnviarMensaje(jsonDisconnected);
            }

            await SacarDeTodosLosCuartos(cliente);
        }

        cliente.Desconectar();
    }

    /// <summary>
    /// Metodo que procesa los dsitintos tipos de mensajes que podamos recibir
    /// </summary>
    /// <param name="cliente"> Es la conexion cliente que envio el mensaje a</param>
    /// <param name="mensaje"> El mensaje que queremos mostrar</param>
    /// 
    public async void MensajeRecibido(ConexionCliente cliente, string mensaje)
    {

        try
        {
            Mensaje? mensajeBase = JsonSerializer.Deserialize<Mensaje>(mensaje);

            if (mensajeBase == null)
            {
                return;
            }

            switch (mensajeBase.type)
            {
                case "IDENTIFY":
                    await ProcesarIdentify(cliente, mensaje);
                    break;
                case "STATUS":
                    await ProcesarStatus(cliente, mensaje);
                    break;
                case "USERS":
                    await ProcesarUsers(cliente, mensaje);
                    break;
                case "PUBLIC_TEXT":
                    await ProcesarPublicText(cliente, mensaje);
                    break;
                case "TEXT":
                    await ProcesarText(cliente, mensaje);
                    break;
                case "NEW_ROOM":
                    await ProcesarNewRoom(cliente, mensaje);
                    break;
                case "INVITE":
                    await ProcesarInvite(cliente, mensaje);
                    break;
                case "JOIN_ROOM":
                    await ProcesarJoinRoom(cliente, mensaje);
                    break;
                case "ROOM_USERS":
                    await ProcesarRoomUsers(cliente, mensaje);
                    break;
                case "ROOM_TEXT":
                    await ProcesarRoomText(cliente, mensaje);
                    break;
                case "LEAVE_ROOM":
                    await ProcesarLeaveRoom(cliente, mensaje);
                    break;
                case "DISCONNECT":
                    await ProcesarDisconnect(cliente, mensaje);
                    break;


                default:
                    if (mensajeBase.type != "IDENTIFY" && mensajeBase.type != null)
                    {
                        await ProcesarNotIdentify(cliente);
                        await CerrarConexion(cliente);
                    }
                    else
                    {
                        await ProcesarInvalid(cliente);
                        await CerrarConexion(cliente);
                    }
                    break;
            }
        }
        catch (JsonException)
        {
            await ProcesarInvalid(cliente);
            await CerrarConexion(cliente);
        }
    }

    /// <summary>
    /// Metodo privado que procesa la intruccion de IDENTIFY enviada al servidor
    /// Verifica que el nombre de usuario no este registrado, en otro casom registra al cliente y notifica la conexion
    /// </summary>
    /// <param name="cliente"> Es la conexion cliente que envio el mensaje a</param>
    /// <param name="mensaje"> Es el mensaje recibido en formato json </param>
    /// <returns> Una tarea que representa la operacion asincrona de procesar un IDENTIFY</returns>
    private async Task ProcesarIdentify(ConexionCliente cliente, string mensaje)
    {
        Identify? identify = JsonSerializer.Deserialize<Identify>(mensaje);

        bool repetido = false;

        foreach (ConexionCliente c in clientes)
        {
            if (c.GetUsername() == identify?.username)
            {
                repetido = true;

                UserAlreadyExist respuesta = new UserAlreadyExist(identify.username);

                string jsonUserAlreadyExists = JsonSerializer.Serialize(respuesta);

                // respuesta del servidor
                await cliente.EnviarMensaje(jsonUserAlreadyExists);

                clientes.Remove(cliente);

                await CerrarConexion(cliente);

                return;
            }
        }

        if (identify?.username.Length > 8)
        {
            await ProcesarInvalid(cliente);
            await CerrarConexion(cliente);
            return;
        }

        if (!repetido)
        {
            if (identify == null)
            {
                return;
            }

            cliente.SetUsername(identify.username);

            cliente.SetStatus("ACTIVE");

            IdentifySuccess identifySuccess = new IdentifySuccess(identify.username);

            string jsonRespuestaSuccess = JsonSerializer.Serialize(identifySuccess);

            string identifyMostrar = JsonSerializer.Serialize(identify);

            await ProcesarNewUser(cliente);

            await cliente.EnviarMensaje(jsonRespuestaSuccess);

            Console.WriteLine(identifyMostrar);
        }
    }

    private async Task ProcesarNewUser(ConexionCliente cliente)
    {
        NewUser newUser = new NewUser(cliente.GetUsername());
        string jsonNewUser = JsonSerializer.Serialize(newUser);

        foreach (ConexionCliente c in clientes)
        {
            if (c != cliente)
            {
                await c.EnviarMensaje(jsonNewUser);
            }
        }
    }
    /// <summary>
    /// Metodo privado que procesa la intruccion de STATUS enviada al servidor
    /// Asigamos el status a nuestro cliente y motrsamos el mensaje de status
    /// </summary>
    /// <param name="cliente"> Es la conexion cliente que envio el mensaje a</param>
    /// <param name="mensaje"> Es el mensaje recibido en formato json </param>
    /// <returns> Una tarea que representa la operacion asincrona de procesar un STATUS</returns>
    private async Task ProcesarStatus(ConexionCliente cliente, string mensaje)
    {
        Status? status = JsonSerializer.Deserialize<Status>(mensaje);

        if (status == null)
        {
            return;
        }

        cliente.SetStatus(status.status);

        NewStatus newStatus = new NewStatus(cliente.GetUsername(), cliente.GetStatus());

        string jsonNewStatus = JsonSerializer.Serialize(newStatus);

        foreach (ConexionCliente c in clientes)
        {
            if (c != cliente)
            {

                await c.EnviarMensaje(jsonNewStatus);
            }
        }

        String mostrarStatus = JsonSerializer.Serialize(status);
        Console.WriteLine(mostrarStatus);
    }

    /// <summary>
    /// Metodo privado que procesa la intruccion de USERS enviada al servidor
    /// Mostramos el mensaje USERS, creamos un diccionario que contenga entrada de username y status
    /// de cada cliente en la lista de clientes obtenemos su nombre y su status y lo agregamos al diccionario
    /// Serializamos como UserList y lo regresamos al cliente
    /// 
    /// </summary>
    /// <param name="cliente"> Es la conexion cliente que envio el mensaje a</param>
    /// <param name="mensaje"> Es el mensaje recibido en formato json </param>
    /// <returns> Una tarea que representa la operacion asincrona de procesar un USERS</returns>
    private async Task ProcesarUsers(ConexionCliente cliente, string mensaje)
    {
        Users? usuario = JsonSerializer.Deserialize<Users>(mensaje);

        string mostrarUser = JsonSerializer.Serialize(usuario);

        Console.WriteLine(mostrarUser);

        Dictionary<string, string> users = new Dictionary<string, string>();

        lock (clientes)
        {

            foreach (ConexionCliente c in clientes)
            {
                string username = c.GetUsername();
                string status = c.GetStatus();

                users.Add(username, status);
            }
        }

        UserList userList = new UserList(users);

        string jsonUserList = JsonSerializer.Serialize(userList);

        await cliente.EnviarMensaje(jsonUserList);
    }

    /// <summary>
    /// Metodo privado que procesa los mensajes distintos a Identify enviados al servidor
    /// Creamos el mensaje notIdentify lo serializamos y lo regresamos al cliente
    /// </summary>
    /// <param name="cliente"> Es la conexion cliente que envio el mensaje a</param>
    /// <returns> Una tarea que representa la operacion asincrona de procesar un NOT_IDENTIFY</returns>
    private async Task ProcesarNotIdentify(ConexionCliente cliente)
    {
        NotIdentify notIdentify = new NotIdentify();
        string jsonNotIdentify = JsonSerializer.Serialize(notIdentify);
        await cliente.EnviarMensaje(jsonNotIdentify);
    }

    /// <summary>
    /// Metodo privado que Procesa la instruccion PUBLIC_TEXT
    /// Manda un mensaje a todas las conexiones menos a si misma
    /// </summary>
    /// <param name="cliente"> Es la conexion del cliente que envio el mensaje </param>
    /// <param name="mensaje"> es el mensaje recibido en formato json </param>
    /// <returns> Una tarea que representa la operacion asincrona de procesar un PUBLIC_TEXT</returns>
    private async Task ProcesarPublicText(ConexionCliente cliente, string mensaje)
    {
        Console.WriteLine(mensaje);

        PublicText? publicText = JsonSerializer.Deserialize<PublicText>(mensaje);

        if (publicText == null)
        {
            return;
        }

        PublicTextFrom publicTextFrom = new PublicTextFrom(cliente.GetUsername(), publicText.text);

        string jsonPublicTextFrom = JsonSerializer.Serialize(publicTextFrom);

        foreach (ConexionCliente c in clientes)
        {
            if (c != cliente)
            {
                await c.EnviarMensaje(jsonPublicTextFrom);
            }
        }
    }

    /// <summary>
    /// Metodo privado que procesa la instruccion TEXT
    /// Manda un mensaje privado al usuario indicado por el mensaje
    /// </summary>
    /// <param name="cliente"> es la conexion del cliente que envio el mensaje </param>
    /// <param name="mensaje"> es el mensaje recibido en formato json </param>
    /// <returns> Una tarea que representa la operacion asincrona de procesar un TEXT</returns>
    private async Task ProcesarText(ConexionCliente cliente, string mensaje)
    {
        // recibimos el mensaje de text
        Console.WriteLine(mensaje);
        PrivText? privText = JsonSerializer.Deserialize<PrivText>(mensaje);

        if (privText == null)
        {
            return;
        }

        // guardamos la informacion
        string usernameDestino = privText.username;
        string usernameOrigen = cliente.GetUsername();
        string textoDelMensaje = privText.text;

        // creamos el textFrom
        PrivTextFrom privTextFrom = new PrivTextFrom(usernameOrigen, textoDelMensaje);
        string jsonPrivTextFrom = JsonSerializer.Serialize(privTextFrom);

        bool usuarioEncontrado = false;

        // enviamos cmo respuesta el PrivTextFrom
        foreach (ConexionCliente c in clientes)
        {
            if (c.GetUsername() == usernameDestino)
            {
                await c.EnviarMensaje(jsonPrivTextFrom);
                usuarioEncontrado = true;
                break;
            }
        }

        // si el usuario no esta en las conexiones
        if (!usuarioEncontrado)
        {
            NoSuchUser noSuchUser = new NoSuchUser("TEXT", usernameDestino);
            string jsonNoSuchUser = JsonSerializer.Serialize(noSuchUser);
            await cliente.EnviarMensaje(jsonNoSuchUser);
        }
    }

    /// <summary>
    /// Metodo privado que procesa una instruccion que no pueda ser identificada por un type
    /// </summary>
    /// <param name="cliente"> es la conexion del cliente que envio el mensaje</param>
    /// <returns> Una tarea que representa la operacion asincrona de procesar un type no valido </returns>
    private async Task ProcesarInvalid(ConexionCliente cliente)
    {
        Invalid invalid = new Invalid();
        string jsonNovalido = JsonSerializer.Serialize(invalid);
        await cliente.EnviarMensaje(jsonNovalido);
    }

    /// <summary>
    /// Metodo privado que procesa la instruccion NEW_ROOM
    /// Crea un nuevo cuarto si pasa las verificaciones indicadas por el protocolo y agrega
    /// al cliente que envio la instruccion
    /// </summary>
    /// <param name="cliente"> es la conexion del cliente que envio el mensaje </param>
    /// <param name="mensaje"> es el mensaje recibido en formato json </param>
    /// <returns> Una tarea que representa la operacion asincrona de procesar un NEW_ROOM</returns>
    private async Task ProcesarNewRoom(ConexionCliente cliente, string mensaje)
    {
        Console.WriteLine(mensaje);
        NewRoom? newRoom = JsonSerializer.Deserialize<NewRoom>(mensaje);

        if (newRoom == null)
        {
            return;
        }
        string roomname = newRoom.roomname;

        if (string.IsNullOrEmpty(roomname) || roomname.Length > 16)
        {
            await ProcesarInvalid(cliente);
            return;
        }

        if (cuartos.ContainsKey(roomname))
        {
            RoomAlreadyExists roomAlreadyExists = new RoomAlreadyExists(newRoom.roomname);
            string jsonRoomAlreadyExits = JsonSerializer.Serialize(roomAlreadyExists);
            await cliente.EnviarMensaje(jsonRoomAlreadyExits);
        }
        else
        {
            Cuarto cuartoNuevo = new Cuarto(roomname);
            cuartoNuevo.AgregarUsuario(cliente);
            cuartos.Add(roomname, cuartoNuevo);

            NewRoomSucess newRoomSucess = new NewRoomSucess(roomname);
            string jsonNewRoomSucces = JsonSerializer.Serialize(newRoomSucess);
            await cliente.EnviarMensaje(jsonNewRoomSucces);
        }
    }

    /// <summary>
    /// Metodo privado que procesa la instruccion INVITE
    /// Tras hacer las verificaciones necesarias, envia una invitacion a un cliente
    /// para unirse a un cuarto
    /// </summary>
    /// <param name="cliente"> es la conexion del cliente que envio el mensaje </param>
    /// <param name="mensaje"> es el mensaje recibido en formato json </param>
    /// <returns> Una tarea que representa la operacion asincrona de procesar un INVITE</returns>
    private async Task ProcesarInvite(ConexionCliente cliente, string mensaje)
    {
        Console.WriteLine(mensaje);
        Invite? invite = JsonSerializer.Deserialize<Invite>(mensaje);

        if (invite == null)
        {
            return;
        }

        string roomname = invite.roomname;
        List<string> usernames = invite.usernames;

        if (string.IsNullOrEmpty(roomname) || usernames == null || usernames.Contains(null))
        {
            await ProcesarInvalid(cliente);
            await CerrarConexion(cliente);
            return;
        }

        List<ConexionCliente> conexionesInvitadas = new List<ConexionCliente>();

        // verifivamos que quien invita esta en el cuarto
        if (!cuartos[roomname].EstaEnElCuarto(cliente))
        {
            await ProcesarInvalid(cliente);
            await CerrarConexion(cliente);
            return;
        }

        // verificamos que el cuarto exista
        if (!cuartos.ContainsKey(roomname))
        {
            NoSuchRoom noSuchRoom = new NoSuchRoom("INVITE", roomname);
            string jsonNoSuchRoom = JsonSerializer.Serialize(noSuchRoom);
            await cliente.EnviarMensaje(jsonNoSuchRoom);
            return;
        }

        // verificamos que cada conexion (nombre) este en la lista de clients
        foreach (string username in usernames)
        {
            ConexionCliente? conexionInvitada = BuscarConexionPorNombre(username);
            if (conexionInvitada == null)
            {
                NoSuchUser noSuchUser = new NoSuchUser("INVITE", username);
                string jsonNoSuchUser = JsonSerializer.Serialize(noSuchUser);
                await cliente.EnviarMensaje(jsonNoSuchUser);
                return;
            }

            conexionesInvitadas.Add(conexionInvitada);
        }

        // todo correcto, enviamos la invitacion
        foreach (ConexionCliente invitado in conexionesInvitadas)
        {
            bool seInvito = cuartos[roomname].Invitar(invitado);

            if (seInvito)
            {
                Invitation invitation = new Invitation(cliente.GetUsername(), roomname);
                string jsonInvitation = JsonSerializer.Serialize(invitation);
                await invitado.EnviarMensaje(jsonInvitation);
            }
        }
    }

    /// <summary>
    /// Metodo privado que procesa la instruccion JOIN_ROOM
    /// Tras hacer las verificaciones necesarias, permite unirse a un cuarto al cliente
    /// </summary>
    /// <param name="cliente"> es la conexion del cliente que envio el mensaje </param>
    /// <param name="mensaje"> es el mensaje recibido en formato json s</param>
    /// <returns> Una tarea que representa la operacion asincrona de procesar un JOIN_ROOM</returns>
    private async Task ProcesarJoinRoom(ConexionCliente cliente, string mensaje)
    {
        Console.WriteLine(mensaje);
        JoinRoom? joinRoom = JsonSerializer.Deserialize<JoinRoom>(mensaje);

        if (joinRoom == null)
        {
            return;
        }

        string roomname = joinRoom.roomname;


        if (string.IsNullOrEmpty(roomname))
        {
            await ProcesarInvalid(cliente);
            await CerrarConexion(cliente);
            return;
        }

        // verificamos que el cuarto exista
        if (!cuartos.ContainsKey(roomname))
        {
            NoSuchRoom noSuchRoom = new NoSuchRoom("JOIN_ROOM", roomname);
            string jsonNoSuchRoom = JsonSerializer.Serialize(noSuchRoom);
            await cliente.EnviarMensaje(jsonNoSuchRoom);
            return;
        }

        List<ConexionCliente> fueronInvitados = cuartos[roomname].GetInvitados();

        if (fueronInvitados.Contains(cliente))
        {
            cuartos[roomname].AgregarUsuario(cliente);
            cuartos[roomname].EliminarInvitado(cliente);
            JoinRoomSuccess joinRoomSuccess = new JoinRoomSuccess(roomname);

            // enviamos al cliente que se pudo unir
            string jsonJoinRoomSuccess = JsonSerializer.Serialize(joinRoomSuccess);
            await cliente.EnviarMensaje(jsonJoinRoomSuccess);

            //notificamos a los demas
            JoinedRoom joinedRoom = new JoinedRoom(roomname, cliente.GetUsername());
            string jsonJoinedRoom = JsonSerializer.Serialize(joinedRoom);

            List<ConexionCliente> estanEnLaSala = cuartos[roomname].GetUsuarios();

            foreach (ConexionCliente c in estanEnLaSala)
            {
                {
                    if (c != cliente)
                    {
                        await c.EnviarMensaje(jsonJoinedRoom);
                    }
                }
            }
        }
        else
        {
            NotInvited notInvited = new NotInvited(roomname);
            string jsonNotInvited = JsonSerializer.Serialize(notInvited);
            await cliente.EnviarMensaje(jsonNotInvited);
        }
    }

    /// <summary>
    /// Metodo privado que procesa la instruccion ROOM_USERS
    /// Tras hacer las verificaciones necesarias, manda la lista de usuarios que hay
    /// en un cuarto junto con sus estados
    /// </summary>
    /// <param name="cliente"> es la conexion del cliente que envio el mensaje </param>
    /// <param name="mensaje"> es el mensaje recibido en formato json </param>
    /// <returns> Una tarea que representa la operacion asincrona de procesar un ROOM_USERS</returns>
    private async Task ProcesarRoomUsers(ConexionCliente cliente, string mensaje)
    {
        Console.WriteLine(mensaje);
        RoomUsers? roomUsers = JsonSerializer.Deserialize<RoomUsers>(mensaje);

        if (roomUsers == null)
        {
            return;
        }

        string roomname = roomUsers.roomname;

        if (string.IsNullOrEmpty(roomname))
        {
            await ProcesarInvalid(cliente);
            await CerrarConexion(cliente);
            return;
        }

        // verificamos que el cuarto exista
        if (!cuartos.ContainsKey(roomname))
        {
            NoSuchRoom noSuchRoom = new NoSuchRoom("ROOM_USERS", roomname);
            string jsonNoSuchRoom = JsonSerializer.Serialize(noSuchRoom);
            await cliente.EnviarMensaje(jsonNoSuchRoom);
            return;
        }

        List<ConexionCliente> usuarios = cuartos[roomname].GetUsuarios();

        if (!usuarios.Contains(cliente))
        {
            NotJoined notJoined = new NotJoined("ROOM_USERS", roomname);
            string jsonNotJoined = JsonSerializer.Serialize(notJoined);
            await cliente.EnviarMensaje(jsonNotJoined);
            return;
        }

        Dictionary<string, string> users = new Dictionary<string, string>();

        foreach (ConexionCliente c in usuarios)
        {
            users.Add(c.GetUsername(), c.GetStatus());
        }

        RoomUserList roomUserList = new RoomUserList(roomname, users);
        string jsonRoomUserList = JsonSerializer.Serialize(roomUserList);
        await cliente.EnviarMensaje(jsonRoomUserList);

    }

    /// <summary>
    /// Metodo privado que procesa la instruccion ROOM_TEXT
    /// Tras hacer las verificaciones necesarias, manda un mensaje a los usuarios 
    /// que hay en un cuarto especifico
    /// </summary>
    /// <param name="cliente"> es la conexion del cliente que envio el menesaje</param>
    /// <param name="mensaje"> es el mensaje recibido en formato json </param>
    /// <returns> Una tarea que representa la operacion asincrona de procesar un ROOM_TEXT</returns>
    private async Task ProcesarRoomText(ConexionCliente cliente, string mensaje)
    {
        Console.WriteLine(mensaje);
        RoomText? roomText = JsonSerializer.Deserialize<RoomText>(mensaje);

        if (roomText == null)
        {
            return;
        }

        string roomname = roomText.roomname;
        string text = roomText.text;
        string username = cliente.GetUsername();

        if (string.IsNullOrEmpty(roomname) || text == null)
        {
            await ProcesarInvalid(cliente);
            await CerrarConexion(cliente);
            return;
        }

        // verificamos que el cuarto exista
        if (!cuartos.ContainsKey(roomname))
        {
            NoSuchRoom noSuchRoom = new NoSuchRoom("ROOM_TEXT", roomname);
            string jsonNoSuchRoom = JsonSerializer.Serialize(noSuchRoom);
            await cliente.EnviarMensaje(jsonNoSuchRoom);
            return;
        }

        List<ConexionCliente> usuarios = cuartos[roomname].GetUsuarios();

        if (!usuarios.Contains(cliente))
        {
            NotJoined notJoined = new NotJoined("ROOM_TEXT", roomname);
            string jsonNotJoined = JsonSerializer.Serialize(notJoined);
            await cliente.EnviarMensaje(jsonNotJoined);
            return;
        }

        RoomTextFrom roomTextFrom = new RoomTextFrom(roomname, username, text);
        string jsonRoomTextFrom = JsonSerializer.Serialize(roomTextFrom);

        foreach (ConexionCliente c in usuarios)
        {
            if (c != cliente)
            {
                await c.EnviarMensaje(jsonRoomTextFrom);
            }
        }
    }

    /// <summary>
    /// Metodo privado que procesa la instruccion LEAVE_ROOM
    /// Tras hacer las verificaciones necesarias, permite que el cliente que manda
    /// la instruccion abandone un cuarto en particular
    /// </summary>
    /// <param name="cliente"> es la conexion del cliente que envio el mensaje </param>
    /// <param name="mensaje"> es el mensaje recibido en formato json</param>
    /// <returns> Una tarea que representa la operacion asincrona de procesar un LEAVE_ROOM</returns>
    private async Task ProcesarLeaveRoom(ConexionCliente cliente, string mensaje)
    {
        Console.WriteLine(mensaje);
        LeaveRoom? leaveRoom = JsonSerializer.Deserialize<LeaveRoom>(mensaje);

        if (leaveRoom == null)
        {
            return;
        }

        string roomname = leaveRoom.roomname;
        string username = cliente.GetUsername();

        if (string.IsNullOrEmpty(roomname))
        {
            await ProcesarInvalid(cliente);
            await CerrarConexion(cliente);
            return;
        }

        // verificamos que el cuarto exista
        if (!cuartos.ContainsKey(roomname))
        {
            NoSuchRoom noSuchRoom = new NoSuchRoom("LEAVE_ROOM", roomname);
            string jsonNoSuchRoom = JsonSerializer.Serialize(noSuchRoom);
            await cliente.EnviarMensaje(jsonNoSuchRoom);
            return;
        }

        List<ConexionCliente> usuarios = cuartos[roomname].GetUsuarios();

        if (!usuarios.Contains(cliente))
        {
            NotJoined notJoined = new NotJoined("LEAVE_ROOM", roomname);
            string jsonNotJoined = JsonSerializer.Serialize(notJoined);
            await cliente.EnviarMensaje(jsonNotJoined);
            return;
        }

        cuartos[roomname].EliminarUsuario(cliente);

        if (cuartos[roomname].EstaVacio())
        {
            cuartos.Remove(roomname);
            return;
        }
        LeftRoom leftRoom = new LeftRoom(roomname, username);
        string jsonLeftRoom = JsonSerializer.Serialize(leftRoom);

        foreach (ConexionCliente c in usuarios)
        {
            if (c != cliente)
            {
                await c.EnviarMensaje(jsonLeftRoom);
            }
        }
    }

    /// <summary>
    /// Metodo privado que procesa la instruccion DISCONNECT
    /// Este metodo desconecta del chat al cliente que envio la instruccion
    /// Avisa a todos los usuarios y ademas abandona los cuartos en que esta el cliente
    /// </summary>
    /// <param name="cliente"></param>
    /// <param name="mensaje"></param>
    /// <returns> Una tarea que representa la operacion asincrona de procesar un DISCONNECT</returns>
    private async Task ProcesarDisconnect(ConexionCliente cliente, string mensaje)
    {
        Console.WriteLine(mensaje);
        await CerrarConexion(cliente);
    }

    /// <summary>
    /// Metodo privado que permite buscar una conexion en la lista de clientes
    /// por su nombre de usuario
    /// </summary>
    /// <param name="username"> es el nombre de la conexion que queremos devolver</param>
    /// <returns> ConexionCliente si encuetra al cliente, null en otro caso</returns>
    private ConexionCliente? BuscarConexionPorNombre(string username)
    {
        foreach (ConexionCliente c in clientes)
        {
            if (c.GetUsername() == username)
            {
                return c;
            }
        }
        return null;
    }

    ///
    /// Operacion asincrona que nos permite iniciar el servidor
    /// </summary>
    /// <returns> Una tarea que representa la operacion asincrona de inicio del servidor </returns>
    public async Task Iniciar()
    {
        await servidor.Iniciar();
    }

}

