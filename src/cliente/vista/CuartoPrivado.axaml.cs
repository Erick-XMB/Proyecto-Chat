using Avalonia.Controls;
using Avalonia.Interactivity;
using ClienteChat.controlador;

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
    /// Constructor de CuartoPrivado
    /// </summary>
    /// <param name="clienteControlador"> es la conexion que tenemos </param>
    public CuartoPrivado(ClienteControlador clienteControlador)
    {
        InitializeComponent();
        this.controlador = clienteControlador;
    }

    /// <sumary>
    /// Metodo que se ejecuta cuando el usaurio hace click en el boton que esta asociado con "Lista_UsuariosCuarto"
    /// <sumary>
    /// <param name="sender">Representa el objeto que producjo el evento, es decir el boton.</param>
    /// <param name="e">contiene la informacion relaciona con el evento que ocurrio.</param>
    private async void Lista_UsuariosCuarto(object? sender, RoutedEventArgs e)
    {

    }

    private async void EnviarInvitacion(object? sender, RoutedEventArgs e)
    {

    }

    private async void Click_DejarCuarto(object? sender, RoutedEventArgs e)
    {

    }

    private async void EnviarMensajeCuarto_Click(object? sender, RoutedEventArgs e)
    {

    }





}