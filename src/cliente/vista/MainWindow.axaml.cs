using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ClienteChat.controlador;

namespace ClienteChat;


/** Clase que hereda de la clase window de avalonia*/
public partial class MainWindow : Window
{
    /// <summary>
    /// Variable que hace referencia a un objeto de tipo ClienteControlador
    /// </summary>
    private readonly ClienteControlador controlador;

    /// <summary>
    /// Variable que hace referencia al username con el que se quiere iniciar sesion
    /// </summary>
    private string username = "";

    /// <summary>
    /// Variable que hace referencia al puerto al que deseamos conectarnos
    /// </summary>
    private int puerto = 0;


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
    private async void Conectar_Click(object? sender, RoutedEventArgs e)
    {
        username = MensajeTextBox.Text;

        puerto = int.Parse(PuertoTextBox.Text);

        /** variable que guarda si se pudo establecer la conexion*/
        bool conectado = controlador.Conectar(puerto);


        /** Si la conexion se hace el */
        if (conectado)
        {
            /** el controlador pasa este mensaje a ConexionCliente y este lo envia usando NetworkStream*/
            if (controlador.Identificar(username))
            {

                controlador.Status("ACTIVE");

                VentanaChat ventanaChat = new VentanaChat(controlador);
                ventanaChat.Show();
                this.Close();

                await controlador.RecibirMensaje();
            }
            else
            {
                this.Close();
            }

        }
        else
        {
            /** Se modifica el texto que tenemos en MainWindow en caso de que la conexion se pudo hacer*/
            EstadoTexto.Text = "Estado: Error de conexión";

        }
    }
}