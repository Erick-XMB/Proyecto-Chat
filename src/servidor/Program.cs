using ServidorChat.Modelo;
using ServidorChat.controlador;

ServidorControlador servidorControlador = new ServidorControlador(5048);

do
{
    await servidorControlador.Iniciar();




} while (servidorControlador.GetServidor().GetActivo());