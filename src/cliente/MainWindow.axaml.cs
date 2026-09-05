using Avalonia.Controls;
using Avalonia.Interactivity;
using Cliente.controlador;

namespace ClienteChat;


/** Clase que hereda de la clase window de avalonia*/
public partial class MainWindow : Window
{   
    /** Variable que hace referencia aun objeto de ClienteCOntrolador*/
    private readonly ClienteControlador controlador;


    /** Constructor de mainWindow*/
    public MainWindow()
    {   
        /** Inicializa los elementos que estan definidios en MainWindow.axaml*/
        InitializeComponent();

        /** Se crea un objeto de tipo clientecontrolador*/
        controlador = new ClienteControlador();
    }


    /// <sumary>
    /// Metodo que se jecuta cuando el usaurio hace click en el boton que esta asociado con "Conectar_CLick"
    /// <sumary>
    /// <param name="sender">Representa el objeto que producjo el evento, es decir el boton.</param>
    /// <param name="e">contiene la informacion relaciona con el evento que ocurrio.</param>
    private void Conectar_Click(object? sender, RoutedEventArgs e)
    {

        controlador.EnviarMensaje("Cliente se conecto");

        bool conectado = controlador.Conectar();

        if (conectado)
        {
            EstadoTexto.Text = "Estado: Conectado";
        }
        else
        {
            EstadoTexto.Text = "Estado: Error de conexión";
        }
    }
}