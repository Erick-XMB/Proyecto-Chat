using Avalonia.Controls;
using Avalonia.Interactivity;
using ClienteChat.controlador;
using protocoloMensajes;
using System.Collections.Generic;


namespace ClienteChat;


/// <summary>
/// Clase que hereda de Window de Avalonia
/// </summary>
public partial class VentanaChat : Window
{

    /// <summary>
    /// Variable que hace referencia aun objeto de ClienteCOntrolador
    /// </summary>
    private readonly ClienteControlador controlador;

    /// <summary>
    /// Variable que hace referencia al nombre de la sala que escribimos en los botones
    /// asociadas a las salas
    /// </summary>
    private string roomnameEscrito = "";

    /// <summary>
    /// Variable que hace referencia al nombre de usuario que escribimos en el boton de 
    /// escribir un mensaje privado
    /// </summary>
    private string usuarioEscrito = "";


    /// <summary>
    /// Constructor de la clase VentanaChat
    /// </summary>
    /// <param name="clienteControlador"> es la conexion que tenemos </param>
    public VentanaChat(ClienteControlador clienteControlador)
    {
        /** Inicializa los elementos que estan definidios en MainWindow.axaml*/
        InitializeComponent();

        this.controlador = clienteControlador;
        this.controlador.MensajeParaInterfaz += MostrarMensaje;

    }

    /// <sumary>
    /// Metodo que se ejecuta cuando el usaurio hace click en el boton que esta asociado con "Lista_Usuarios"
    /// <sumary>
    /// <param name="sender">Representa el objeto que producjo el evento, es decir el boton.</param>
    /// <param name="e">contiene la informacion relaciona con el evento que ocurrio.</param>
    private async void Lista_Usuarios(object? sender, RoutedEventArgs e)
    {
        controlador.Users();
        UsuariosTextBox.Clear();
    }

    /// <summary>
    /// Metodo asociado con el boton de enviar mensaje privado, abimos una ventana privada
    /// se introduce el usuario, verificamos al usuario, y llamamos a crear la conversacion privada
    /// </summary>
    /// <param name="sender">Representa el objeto que producjo el evento, es decir el boton</param>
    /// <param name="e">contiene la informacion relaciona con el evento que ocurrio</param>
    private async void MensajePrivado_Click(object? sender, RoutedEventArgs e)
    {
        var cajaDeTextoEscribirPrivado = this.FindControl<TextBox>("EscribirPrivado");

        if (cajaDeTextoEscribirPrivado == null)
            return;

        usuarioEscrito = cajaDeTextoEscribirPrivado.Text ?? "";

        if (string.IsNullOrWhiteSpace(usuarioEscrito))
        {
            return;
        }

        CrearConversacionPrivada(usuarioEscrito, true);
    }

    /// <summary>
    /// Metodo que nos permite que al dar click en UnirseCuarto mandemos el mensaje de JoinRoom 
    /// al servidor
    /// </summary>
    /// <param name="sender">Representa el objeto que producjo el evento, es decir el boton</param>
    /// <param name="e">contiene la informacion relaciona con el evento que ocurrio</param>
    private async void UnirseCuarto_Click(object? sender, RoutedEventArgs e)
    {

        var cajaDeTextoEscribirCuarto = this.FindControl<TextBox>("UnirseCuarto");

        if (cajaDeTextoEscribirCuarto == null)
            return;

        roomnameEscrito = cajaDeTextoEscribirCuarto.Text ?? "";
        if (string.IsNullOrWhiteSpace(roomnameEscrito))
        {
            return;
        }

        controlador.JoinRoom(roomnameEscrito);
        cajaDeTextoEscribirCuarto.Clear();
    }

    /// <summary>
    /// Metodo asocidado con el boton de Crear Cuarto, es analogo a MensajePrivado_Click es solo
    /// que aqui no creamos el tab del cuarto
    /// </summary>
    /// <param name="sender">Representa el objeto que producjo el evento, es decir el boton</param>
    /// <param name="e">contiene la informacion relaciona con el evento que ocurrio</param>
    private void CrearCuarto_Click(object? sender, RoutedEventArgs e)
    {
        //VentanaCuarto ventana = new VentanaCuarto(controlador);
        //string? roomname = await ventana.ShowDialog<string?>(this);

        var cajaDeTextoEscribirCuarto = this.FindControl<TextBox>("EscribirCuarto");

        if (cajaDeTextoEscribirCuarto == null)
            return;

        roomnameEscrito = cajaDeTextoEscribirCuarto.Text ?? "";
        if (string.IsNullOrWhiteSpace(roomnameEscrito))
        {
            return;
        }

        controlador.NewRoom(roomnameEscrito);
        cajaDeTextoEscribirCuarto.Clear();
    }

    /// <summary>
    /// Metodo que nos permite crear una nueva pestania con la interfaz que hicimos
    /// para el cuarto privado
    /// </summary>
    /// <param name="roomname"> es el nombre del cuarto </param>
    private void CrearCuartoPrivado(string roomname)
    {
        foreach (TabItem? tabItem in ConversacionesTabControl.Items)
        {
            if (tabItem?.Tag?.ToString() == roomname)
            {
                return;
            }
        }

        TabItem nuevaPestana = new TabItem
        {
            Header = roomname,
            Tag = roomname,
            Content = new CuartoPrivado(controlador, roomname)
        };

        ConversacionesTabControl.Items.Add(nuevaPestana);
    }

    /// <summary>
    /// Metooo que nos permite crear una nueva conversacion privada
    /// Buscamos que no haya una pestania asociada ya con el nombre del usuario a quien le queremos escribir el mensaje
    /// Creamos una nueva pestania con la interfaz de ChatPrivado
    /// </summary>
    /// <param name="usuario"> es el nombre de lusuario </param>
    /// <param name="seleccionarConversacion"> booleano que nos permite tener control sobre las pestanias en las que estamos </param>
    /// <param name="mensajeInicial"></param> Mensaje de tipo PrivTextFrom que nos permite asociado a el pirmer mensaje que llega a la conexion<summary>
    private async void CrearConversacionPrivada(string usuario, bool seleccionarConversacion, PrivTextFrom? mensajeInicial = null)
    {
        foreach (TabItem? tabItem in ConversacionesTabControl.Items)
        {
            if (tabItem?.Tag?.ToString() == usuario)
            {
                return;
            }
        }

        TabItem nuevaPestana = new TabItem
        {
            Header = usuario,
            Tag = usuario,
            Content = new ChatPrivado(controlador, usuario, mensajeInicial)
        };

        ConversacionesTabControl.Items.Add(nuevaPestana);

        if (seleccionarConversacion)
        {
            ConversacionesTabControl.SelectedItem = nuevaPestana;
        }
    }


    /// <summary>
    /// Metodo que esta asociado al boton de desconectar, hacemos la desconeccion del cliente 
    /// y mandamos disconnected a los demas clientes
    /// </summary>
    /// <param name="sender">Representa el objeto que producjo el evento, es decir el boton</param>
    /// <param name="e">Contiene la informacion relaciona con el evento que ocurrio</param>
    private async void Click_Desconectar(object? sender, RoutedEventArgs e)
    {
        controlador.Disconnect();
        this.Close();
    }

    /// <summary>
    /// Metodo que esta asociado al boton de Active, cambiamos el estado del cliente a Active
    /// </summary>
    /// <param name="sender">Representa el objeto que producjo el evento, es decir el boton</param>
    /// <param name="e">Contiene la informacion relaciona con el evento que ocurrio</param>
    public async void OpcionActive(object? sender, RoutedEventArgs e)
    {
        controlador.Status("ACTIVE");
    }

    /// <summary>
    /// Metodo que esta asociado al boton de Active, cambiamos el estado del cliente a Away
    /// </summary>
    /// <param name="sender">Representa el objeto que producjo el evento, es decir el boton</param>
    /// <param name="e">Contiene la informacion relaciona con el evento que ocurrio</param>
    public async void OpcionAway(object? sender, RoutedEventArgs e)
    {
        controlador.Status("AWAY");
    }

    /// <summary>
    /// Metodo que esta asociado al boton de Active, cambiamos el estado del cliente a Busy
    /// </summary>
    /// <param name="sender">Representa el objeto que producjo el evento, es decir el boton</param>
    /// <param name="e">Contiene la informacion relaciona con el evento que ocurrio</param>
    public async void OpcionBusy(object? sender, RoutedEventArgs e)
    {
        controlador.Status("BUSY");
    }

    /// <summary>
    /// Metodo que esta asociado al boton de enviar
    /// Serializamos a PublicText el texto
    /// </summary>
    /// <param name="sender">epresenta el objeto que producjo el evento, es decir el boton</param>
    /// <param name="e">Contiene la informacion relaciona con el evento que ocurrio</param>
    public async void EnviarMensaje_Click(object? sender, RoutedEventArgs e)
    {

        string? text = EnviarMensajeTextBox.Text;

        if (string.IsNullOrWhiteSpace(text))
        {
            return;
        }

        controlador.PublicText(text);

        MensajesTextBox.Text += $"Yo: {text}\n";

        EnviarMensajeTextBox.Clear();
    }

    /// <summary>
    /// Metodo que nos permite dado un mensaje, manejar el tipo de mensaje que es
    /// ya sea para mostrarlo en la terminal, o para hacer operaciones asociadas
    /// como crear conversaciones privadas o salas
    /// </summary>
    /// <param name="mensaje"> es el mensaje que estamos recibiendo </param>
    private void MostrarMensaje(Mensaje mensaje)
    {
        switch (mensaje)
        {
            case NewUser newUser:
                MensajesTextBox.Text += $"{newUser.username} joined the chat...\n";
                break;

            case PublicTextFrom publicTextFrom:
                MensajesTextBox.Text += $"{publicTextFrom.username}: {publicTextFrom.text}\n";
                break;

            case Disconnected disconnected:
                MensajesTextBox.Text += $"{disconnected.username} has left the chat...\n";
                break;

            case NewStatus newStatus:
                MensajesTextBox.Text += $"{newStatus.username} now is {newStatus.status}\n";
                break;

            case UserList userList:
                foreach (KeyValuePair<string, string> par in userList.users)
                {
                    UsuariosTextBox.Text += $"{par.Key}: {par.Value}\n";
                }
                break;

            case PrivTextFrom privTextFrom:
                CrearConversacionPrivada(privTextFrom.username, false, privTextFrom);
                break;

            case NewRoomSucess newRoomSucess:
                CrearCuartoPrivado(newRoomSucess.extra);
                break;

            case RoomAlreadyExists roomAlreadyExists:
                MensajesTextBox.Text += $"La sala {roomAlreadyExists.extra} YA EXISTE\n";
                break;

            case NoSuchRoom noSuchRoom when noSuchRoom.operation == "JOIN_ROOM":
                MensajesTextBox.Text += $"La sala {noSuchRoom.extra} NO EXISTE\n";
                break;

            case Invitation invitation:
                var cajaDeTextoEscribirCuarto = this.FindControl<TextBox>("InvitacionesCuarto");

                if (cajaDeTextoEscribirCuarto == null)
                {
                    return;
                }
                
                cajaDeTextoEscribirCuarto.Text += $"INVITADO A '{invitation.roomname}' POR '{invitation.username}' \n";
                break;

            case JoinRoomSuccess joinRoomSuccess:
                CrearCuartoPrivado(joinRoomSuccess.extra);
                break;

            case NotInvited notInvited:
                MensajesTextBox.Text += $"NO FUISTE INVITADO A LA SALA '{notInvited.extra}'";
                break;

            case Response response:
                if (response.result == "INVALID")
                {
                    controlador.Disconnect();
                    this.Close();
                }
                break;
        }

    }



}