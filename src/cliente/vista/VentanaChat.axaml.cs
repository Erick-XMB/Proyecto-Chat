using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ClienteChat.controlador;

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

    }

    /// <sumary>
    /// Metodo que se ejecuta cuando el usaurio hace click en el boton que esta asociado con "Lista_Usuarios"
    /// <sumary>
    /// <param name="sender">Representa el objeto que producjo el evento, es decir el boton.</param>
    /// <param name="e">contiene la informacion relaciona con el evento que ocurrio.</param>
    private async void Lista_Usuarios(object? sender, RoutedEventArgs e)
    {
        controlador.Users();
        await controlador.RecibirMensaje();
    }
}