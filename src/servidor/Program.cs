using ServidorChat.Modelo;
using ServidorChat.controlador;

int puerto = int.Parse(args[0]);

ServidorControlador servidorControlador = new ServidorControlador(puerto);

do
{
    await servidorControlador.Iniciar();




} while (servidorControlador.GetServidor().GetActivo());