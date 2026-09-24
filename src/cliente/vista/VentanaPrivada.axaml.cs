using Avalonia.Controls;
using Avalonia.Interactivity;

namespace ClienteChat;

/// <summary>
/// Clase VentanaPrivada que hereda de Window
/// </summary>
public partial class VentanaPrivada : Window
{

    /// <summary>
    /// Constructor de la clase VentanaPrivadaI
    /// </summary>
    public VentanaPrivada()
    {
        /** Inicializa los elementos que estan definidios en MainWindow.axaml*/
        InitializeComponent();

    }

    /// <summary>
    /// Metoddo que permite hacer una accion al presionar el boton aceptar
    /// asociamos el nombre del usuarios
    /// </summary>
    /// <param name="sender">Representa el objeto que producjo el evento, es decir el boton</param>
    /// <param name="e">Contiene la informacion relaciona con el evento que ocurrio</param>
    private void Aceptar_Click(object? sender, RoutedEventArgs e)
    {
        string usuario = UsuarioTextBox.Text;

        if (string.IsNullOrWhiteSpace(usuario))
        {
            this.Close();
            return;
        }

        Close(usuario);
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