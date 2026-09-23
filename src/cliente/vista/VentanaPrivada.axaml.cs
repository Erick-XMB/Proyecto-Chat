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
public partial class VentanaPrivada : Window
{

    /** Constructor de VentanaChat*/
    public VentanaPrivada()
    {
        /** Inicializa los elementos que estan definidios en MainWindow.axaml*/
        InitializeComponent();
    }

    private void Aceptar_Click(object? sender, RoutedEventArgs e)
    {
        string usuario = UsuarioTextBox.Text;

        if (string.IsNullOrWhiteSpace(usuario))
        {
            this.Close();
        }

        Close(usuario);
    }

    private void Cancelar_Click(object? sender, RoutedEventArgs e)
    {
        Close(null);
    }
}