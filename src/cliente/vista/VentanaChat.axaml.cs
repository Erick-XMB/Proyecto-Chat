using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ClienteChat.controlador;
using System.Text.Json;
using protocoloMensajes;
using System.Collections.Generic;
using Avalonia.Metadata;
using Avalonia.Media;

namespace ClienteChat;




/** Clase que hereda de la clase window de avalonia*/
public partial class VentanaChat : Window
{

    /** Variable que hace referencia aun objeto de ClienteCOntrolador*/
    private readonly ClienteControlador controlador;


    /** Constructor de VentanaChat*/
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

    private async void MensajePrivado_Click(object? sender, RoutedEventArgs e)
    {
        VentanaPrivada ventana = new VentanaPrivada();
        string? usuario = await ventana.ShowDialog<string?>(this);

        if (string.IsNullOrWhiteSpace(usuario))
        {
            return;
        }

        CrearConversacionPrivada(usuario, true);
    }

    private async void CrearCuarto_Click(object? sender, RoutedEventArgs e)
    {
        VentanaCuarto ventana = new VentanaCuarto();
        string? cuarto = await ventana.ShowDialog<string?>(this);

        if (string.IsNullOrWhiteSpace(cuarto))
        {
            return;
        }
    }

    private async void CrearConversacionPrivada(string usuario, bool seleccionarConversacion, PrivTextFrom? mensajeInicial = null)
    {

        foreach (TabItem? tabItem in ConversacionesTabControl.Items)
        {
            if (tabItem.Tag?.ToString() == usuario)
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


    private async void Click_Desconectar(object? sender, RoutedEventArgs e)
    {
        controlador.Disconnect();
        this.Close();
    }

    public async void OpcionActive(object? sender, RoutedEventArgs e)
    {
        controlador.Status("ACTIVE");
    }

    public async void OpcionAway(object? sender, RoutedEventArgs e)
    {
        controlador.Status("AWAY");
    }

    public async void OpcionBusy(object? sender, RoutedEventArgs e)
    {
        controlador.Status("BUSY");
    }

    public async void EnviarMensaje_Click(object? sender, RoutedEventArgs e)
    {
        string? text = EnviarMensajeTextBox.Text;

        controlador.PublicText(text);

        MensajesTextBox.Text += $"Yo: {text}\n";

        EnviarMensajeTextBox.Clear();
    }

    private async void MostrarMensaje(Mensaje mensaje)
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
        }
    }


}