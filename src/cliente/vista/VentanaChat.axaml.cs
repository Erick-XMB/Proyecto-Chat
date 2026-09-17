using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ClienteChat.controlador;

namespace ClienteChat;




/** Clase que hereda de la clase window de avalonia*/
public partial class VentanaChat : Window
{

    private readonly ClienteControlador controlador;
    public VentanaChat(ClienteControlador clienteControlador)
    {
        /** Inicializa los elementos que estan definidios en MainWindow.axaml*/
        InitializeComponent();

        this.controlador = clienteControlador;

    }

    private async void Lista_Usuarios(object? sender, RoutedEventArgs e)
    {
        controlador.Users();
        await controlador.RecibirMensaje();
    }
}