using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ClienteChat.controlador;
using protocoloMensajes;

namespace ClienteChat;




/// <summary>
/// Clase que hereda de UserControl de avalonia
/// </summary>
public partial class CuartoPrivado : UserControl
{

    /// <summary>
    /// Variable que hace referencia aun objeto de ClienteCOntrolador
    /// </summary>
    private readonly ClienteControlador controlador;

    /// <summary>
    /// Atributo que representa el nombre de la pestania asociada al cuarto
    /// </summary> 
    private string roomname;

    /// <summary>
    /// Atributo que usamos para representar los nombres de usuario que escribimos
    /// </summary>
    private string nombresDeUsuario = "";


    /// <summary>
    /// Constructor de CuartoPrivado
    /// </summary>
    /// <param name="clienteControlador"> es la conexion que tenemos </param>
    public CuartoPrivado(ClienteControlador clienteControlador, string roomname)
    {
        InitializeComponent();
        this.controlador = clienteControlador;
        this.roomname = roomname;
        this.controlador.MensajeParaInterfaz += ProcesarMensaje;
    }


    /// <summary>
    /// Metodo que nos permite dado un mensaje, manejar el tipo de mensaje que es
    /// ya sea para mostrarlo en la interfaz
    /// </summary>
    /// <param name="mensaje"> es el mensaje que estamos recibiendo </param>
    private void ProcesarMensaje(Mensaje mensaje)
    {
        switch (mensaje)
        {
            case NoSuchUser noSuchUser:
                MensajesCuartoTextBox.Text += $"ERROR AL ENVIAR LAS INVITACIONES, USUARIO {noSuchUser.extra} NO ENCONTRADO\n";
                break;
            case NoSuchRoom noSuchRoom when noSuchRoom.operation == "JOIN_ROOM" && noSuchRoom.extra == this.roomname:
                MensajesCuartoTextBox.Text += $"{noSuchRoom.extra} NO ENCONTRADO\n";
                break;
            case JoinedRoom joinedRoom when joinedRoom.roomname == this.roomname:
                MensajesCuartoTextBox.Text += $"{joinedRoom.username} joined the room...\n";
                break;

            case RoomUserList roomUserList when roomUserList.roomname == this.roomname:
                foreach (KeyValuePair<string, string> par in roomUserList.users)
                {
                    UsuariosCuartoTextBox.Text += $"{par.Key}: {par.Value}\n";
                }
                break;

            case NoSuchRoom noSuchRoom when noSuchRoom.operation == "ROOM_USERS" && noSuchRoom.extra == this.roomname:
                MensajesCuartoTextBox.Text += $"{noSuchRoom.extra} NO ENCONTRADO\n";
                break;

            case NotJoined notJoined when notJoined.operation == "ROOM_USERS" && notJoined.extra == this.roomname:
                MensajesCuartoTextBox.Text += $"NO HAS SIDO INVITADO A {notJoined.extra}\n";
                break;

            case LeftRoom leftRoom when leftRoom.roomname == this.roomname:
                MensajesCuartoTextBox.Text += $"{leftRoom.username} has left the room...\n";
                break;

            case NoSuchRoom noSuchRoom when noSuchRoom.operation == "LEAVE_ROOM" && noSuchRoom.extra == this.roomname:
                MensajesCuartoTextBox.Text += $"{noSuchRoom.extra} NO ENCONTRADO\n";
                break;

            case NotJoined notJoined when notJoined.operation == "LEAVE_ROOM" && notJoined.extra == this.roomname:
                MensajesCuartoTextBox.Text += $"NO HAS SIDO INVITADO A {notJoined.extra}\n";
                break;
            case RoomTextFrom roomTextFrom when roomTextFrom.roomname == this.roomname:
                MensajesCuartoTextBox.Text += $"{roomTextFrom.username}: {roomTextFrom.text}\n";
                break;

            case NoSuchRoom noSuchRoom when noSuchRoom.operation == "ROOM_TEXT" && noSuchRoom.extra == this.roomname:
                MensajesCuartoTextBox.Text += $"{noSuchRoom.extra} NO ENCONTRADO\n";
                break;

            case NotJoined notJoined when notJoined.operation == "ROOM_TEXT" && notJoined.extra == this.roomname:
                MensajesCuartoTextBox.Text += $"NO HAS SIDO INVITADO A {notJoined.extra}\n";
                break;

        }
    }

    /// <sumary>
    /// Metodo que se ejecuta cuando el usaurio hace click en el boton que esta asociado con "Lista_UsuariosCuarto"
    /// <sumary>
    /// <param name="sender">Representa el objeto que producjo el evento, es decir el boton.</param>
    /// <param name="e">contiene la informacion relaciona con el evento que ocurrio.</param>
    private void Lista_UsuariosCuarto(object? sender, RoutedEventArgs e)
    {
        controlador.RoomUsers(this.roomname);
        UsuariosCuartoTextBox.Clear();
    }

    /// <summary>
    /// Metodo que nos permite que al hacer click Enviar invitacion, guardemos los nombres
    /// de usuario escritos e intentemos mandar el mensaje deINVITE
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <returns></returns>
    private void EnviarInvitacion_Click(object? sender, RoutedEventArgs e)
    {
        var cajaDeTextoEscribirCuarto = this.FindControl<TextBox>("EscrbirNombresDeInvitados");

        if (cajaDeTextoEscribirCuarto == null)
        return;


        nombresDeUsuario = cajaDeTextoEscribirCuarto.Text ?? "";

        if (string.IsNullOrWhiteSpace(nombresDeUsuario))
        {
            return;
        }

        string[] invtadosNombres = nombresDeUsuario.Split(',');

        List<string> invitadosLista = new List<string>();

        foreach (string usuario in invtadosNombres)
        {
            invitadosLista.Add(usuario);
        }

        controlador.Invite(roomname, invitadosLista);
        cajaDeTextoEscribirCuarto.Clear();
    }

    /// <summary>
    /// Metodo que nos permie que al dar click en Dejar cuarto abandonemos el cuarto
    /// correspondiente
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <returns></returns>
    private void Click_DejarCuarto(object? sender, RoutedEventArgs e)
    {
        controlador.LeaveRoom(this.roomname);
    }

    /// <summary>
    /// Metodo que nos permite enviar mensaje al textbox de la sala privada
    /// </summary>
    /// <param name="sender">Representa el objeto que producjo el evento, es decir el boton.</param>
    /// <param name="e">contiene la informacion relaciona con el evento que ocurrio.</param>
    private void EnviarMensajeCuarto_Click(object? sender, RoutedEventArgs e)
    {

        string? text = EnviarMensajeCuartoTextBox.Text;

        if (string.IsNullOrWhiteSpace(text))
        {
            return;
        }

        controlador.RoomText(this.roomname, text);

        MensajesCuartoTextBox.Text += $"Yo: {text}\n";

        EnviarMensajeCuartoTextBox.Clear();
    }
}