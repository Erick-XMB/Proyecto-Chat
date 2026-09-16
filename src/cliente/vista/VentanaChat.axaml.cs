using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ClienteChat.controlador;

namespace ClienteChat;


/** Clase que hereda de la clase window de avalonia*/
public partial class VentanaChat : Window
{
    public VentanaChat()
    {
        /** Inicializa los elementos que estan definidios en MainWindow.axaml*/
        InitializeComponent();

    }
}