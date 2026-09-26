using Avalonia.Controls;
using Avalonia.Interactivity;
using ClienteChat.controlador;
using protocoloMensajes;
namespace ClienteChat;

/** Clase que hereda de la clase UserControl de avalonia*/
public partial class ChatPrivado : UserControl
{
    /// <summary>
    /// Variable que hace referencia a un objeto de tipo ClienteControlador
    /// </summary>
    private readonly ClienteControlador controlador;

    /// <summary>
    /// Es el nombre de usuario con quien mantenemos el chat privado
    /// </summary>
    private string username;

    /// <summary>
    /// Constructor de la clase ChatPrivado
    /// </summary>
    /// <param name="clienteControlador">es la conexion que tenemos</param>
    /// <param name="username"> es el nombre con quien haremos el chat privado </param>
    /// <param name="primerMensaje"> mensaje de tipo PrivTextFrom que nos permite mostrar el pirmer mensaje que llega a la conexion </param>    
    public ChatPrivado(ClienteControlador clienteControlador, string username, PrivTextFrom? primerMensaje = null)
    {
        /** Inicializa los elementos que estan definidios en MainWindow.axaml*/
        InitializeComponent();
        this.controlador = clienteControlador;
        this.controlador.MensajeParaInterfaz += RecibirMensaje;
        this.username = username;
        if (primerMensaje != null)
        {
            MensajesPrivadosTextBox.Text += $"{primerMensaje.username}: {primerMensaje.text}\n";
        }
    }

    /// <summary>
    /// Metodo que dado un mensaje recibido maneja los distintos tipos de casos que hay 
    /// </summary>
    /// <param name="mensaje"> es el mensaje que estamos recibiendpo</param>
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

    /// <summary>
    /// Metodo que nos permite hacer click en EnviarMensajePrivado, 
    /// Mandamos la instruccion de privText ademas de mostrar el mensaje en pantalla
    /// </summary>
    /// <param name="sender">Representa el objeto que producjo el evento, es decir el boton</param>
    /// <param name="e">contiene la informacion relaciona con el evento que ocurrio.</param>
    /// <returns></returns>
    private async void EnviarMensajePrivado_Click(object? sender, RoutedEventArgs e)
    {
        string? text = EnviarMensajePrivadoTextBox.Text;

        if (string.IsNullOrWhiteSpace(text))
        {
            return;
        }

        controlador.PrivText(username, text);

        MensajesPrivadosTextBox.Text += $"Yo: {text}\n";

        EnviarMensajePrivadoTextBox.Clear();
    }

}