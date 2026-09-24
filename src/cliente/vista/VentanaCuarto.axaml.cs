using Avalonia.Controls;
using Avalonia.Interactivity;
using ClienteChat.controlador;

namespace ClienteChat;

/// <summary>
/// Clase que hereda de Window de avalonia
/// </summary>
public partial class VentanaCuarto : Window
{
    /// <summary>
    /// Variable que hace referencia aun objeto de ClienteCOntrolador
    /// </summary>
    private readonly ClienteControlador controlador;

    /// <summary>
    /// Constructor de la clase VentanaCuarto
    /// </summary>
    /// <param name="controlador"> Es la conexion que se esta manejando </param>
    public VentanaCuarto(ClienteControlador controlador)
    {
        InitializeComponent();
        this.controlador = controlador;

    }

    /// <summary>
    /// Metoddo que permite hacer una accion al presionar el boton aceptar
    /// Mandamos el mensaje de NEW_ROOM ademas de que asociamos el nombre
    /// a roomname
    /// </summary>
    /// <param name="sender">Representa el objeto que producjo el evento, es decir el boton</param>
    /// <param name="e">Contiene la informacion relaciona con el evento que ocurrio</param>
    private void Aceptar_Click(object? sender, RoutedEventArgs e)
    {
        string roomname = CuartoTextBox.Text;

        if (string.IsNullOrWhiteSpace(roomname))
        {
            this.Close();
            return;
        }

        controlador.NewRoom(roomname);
        Close(roomname);
    }

    /// <summary>
    /// Metodo que nos permite hacer una accion a presionar el boton de cancelar
    /// hacemos que solo se asocie null
    /// </summary>
    /// <param name="sender">Representa el objeto que producjo el evento, es decir el boton</param>
    /// <param name="e">Contiene la informacion relaciona con el evento que ocurrio</param>
    private void Cancelar_Click(object? sender, RoutedEventArgs e)
    {
        Close(null);
    }
}