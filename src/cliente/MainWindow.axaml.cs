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

        /** variable que guarda si se pudo establecer la conexion*/
        bool conectado = controlador.Conectar();


        /** Si la conexion se hace el */
        if (conectado)
        {
            /** el controlador pasa este mensaje a ConexionCliente y este lo envia usando NetworkStream*/
            controlador.EnviarMensaje("Cliente se conecto");

            /** Se modificsa el texto que tenemos en MainWindow.axml*/
            EstadoTexto.Text = "Estado: Conectado";



        }
        else
        {
            /** Se modifica el texto que tenemos en MainWindow en caso de que la conexion se pudo hacer*/
            EstadoTexto.Text = "Estado: Error de conexión";

        }
    }



}