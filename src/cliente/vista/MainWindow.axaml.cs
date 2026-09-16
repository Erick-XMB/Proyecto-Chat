using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ClienteChat.controlador;

namespace ClienteChat;


/** Clase que hereda de la clase window de avalonia*/
public partial class MainWindow : Window
{
    /** Variable que hace referencia aun objeto de ClienteCOntrolador*/
    private readonly ClienteControlador controlador;

    private string username = "";


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

        /** variable que guarda si se pudo establecer la conexion*/
        bool conectado = controlador.Conectar(5085);


        /** Si la conexion se hace el */
        if (conectado)
        {
            /** el controlador pasa este mensaje a ConexionCliente y este lo envia usando NetworkStream*/
            controlador.Identificar(username);

            VentanaChat ventanaChat = new VentanaChat();
            ventanaChat.Show();

            Close();


            await controlador.RecibirMensaje();





        }
        else
        {
            /** Se modifica el texto que tenemos en MainWindow en caso de que la conexion se pudo hacer*/
            EstadoTexto.Text = "Estado: Error de conexión";

        }
    }
}