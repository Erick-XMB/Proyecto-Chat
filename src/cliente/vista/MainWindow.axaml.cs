using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ClienteChat.controlador;
using protocoloMensajes;

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

    /// <summary>
    /// Atributo que nos permite esperar el resultado de identificacion de un usuario
    /// </summary>
    private TaskCompletionSource<bool>? identificacionTCS;

    /// <summary>
    /// Constructor de la clase MainWindow
    /// </summary>
    public MainWindow()
    {
        /** Inicializa los elementos que estan definidios en MainWindow.axaml*/
        InitializeComponent();

        /** Se crea un objeto de tipo clientecontrolador*/
        controlador = new ClienteControlador();
        controlador.MensajeParaInterfaz += MensajeRecibido;
    }

    /// <summary>
    /// Metodo que dado un mensaje recibido modifica los resultados de la identificacion
    /// esto para poder iniciar sesion
    /// </summary>
    /// <param name="mensaje"> es el mensaje que etsamos recibiendo </param>
    private async void MensajeRecibido(Mensaje mensaje)
    {
        switch (mensaje)
        {
            case IdentifySuccess identifySuccess:
                identificacionTCS?.TrySetResult(true);
                break;

            case UserAlreadyExist userAlreadyExist:
                identificacionTCS?.TrySetResult(false);
                break;

            case NotIdentify notIdentify:
                identificacionTCS?.TrySetResult(false);
                break;
            case Invalid invalid:
                identificacionTCS?.TrySetResult(false);
                break;
        }
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
            _ = controlador.RecibirMensajes();

            identificacionTCS = new TaskCompletionSource<bool>();

            /** el controlador pasa este mensaje a ConexionCliente y este lo envia usando NetworkStream*/
            controlador.Identificar(username);

            bool identificacionSuccess = await identificacionTCS.Task;

            if (identificacionSuccess)
            {
                controlador.Status("ACTIVE");
                VentanaChat ventanaChat = new VentanaChat(controlador);
                ventanaChat.Show();
                this.Close();
            } else
            {
                controlador.Desconectar();
                EstadoTexto.Text = "Estado: Error en la identidicacion";
                this.Close();
            }
        }
        else
        {   
            controlador.Desconectar();
            /** Se modifica el texto que tenemos en MainWindow en caso de que la conexion se pudo hacer*/
            EstadoTexto.Text = "Estado: Error de conexión";

        }

    }
}