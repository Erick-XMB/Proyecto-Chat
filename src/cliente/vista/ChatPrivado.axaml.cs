using System;

using System.Threading.Tasks;

using Avalonia.Controls;

using Avalonia.Interactivity;

using ClienteChat.controlador;

using System.Text.Json;

using protocoloMensajes;

using System.Collections.Generic;

using Avalonia.Metadata;

namespace ClienteChat;

/** Clase que hereda de la clase window de avalonia*/

public partial class ChatPrivado : UserControl

{

    /** Constructor de VentanaChat*/

    /** Variable que hace referencia aun objeto de ClienteCOntrolador*/

    private readonly ClienteControlador controlador;

    private string username;

    public ChatPrivado(ClienteControlador clienteControlador, string username, PrivTextFrom? primerMensaje = null)

    {

        /** Inicializa los elementos que estan definidios en MainWindow.axaml*/

        InitializeComponent();

        this.controlador = clienteControlador;

        this.controlador.MensajeParaInterfaz += RecibirMensaje;

        this.username = username;

        if(primerMensaje != null)
        {
            MensajesPrivadosTextBox.Text += $"{primerMensaje.username}: {primerMensaje.text}\n";
        }

    }

    private async void RecibirMensaje(Mensaje mensaje)
    {
        switch (mensaje)
        {
            case PrivTextFrom privTextFrom when privTextFrom.username == this.username:
                MensajesPrivadosTextBox.Text += $"{privTextFrom.username}: {privTextFrom.text}\n";
                break;
            case NoSuchUser noSuchUser when noSuchUser.extra == this.username:
                MensajesPrivadosTextBox.Text += $"ERROR: USUARIO {noSuchUser.extra} NO ENCONTRADO\n";
                MensajesPrivadosTextBox.IsEnabled = false;
                break;
        }
    }

    private async void EnviarMensajePrivado_Click(object? sender, RoutedEventArgs e)
    {
        string? text = EnviarMensajePrivadoTextBox.Text;

        controlador.PrivText(username, text);

        MensajesPrivadosTextBox.Text += $"Yo: {text}\n";

        EnviarMensajePrivadoTextBox.Clear();
    }

}