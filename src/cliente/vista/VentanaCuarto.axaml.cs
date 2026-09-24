using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ClienteChat.controlador;
using System.Text.Json;
using protocoloMensajes;
using System.Collections.Generic;
using Avalonia.Metadata;

namespace ClienteChat;

/** Clase que hereda de la clase window de avalonia*/
public partial class VentanaCuarto : Window
{

    /** Constructor de VentanaChat*/
    public VentanaCuarto()
    {
        /** Inicializa los elementos que estan definidios en MainWindow.axaml*/
        InitializeComponent();
    }

    private void Aceptar_Click(object? sender, RoutedEventArgs e)
    {
        string cuarto = CuartoTextBox.Text;

        if (string.IsNullOrWhiteSpace(cuarto))
        {
            this.Close();
        }

        Close(cuarto);
    }

    private void Cancelar_Click(object? sender, RoutedEventArgs e)
    {
        Close(null);
    }
}