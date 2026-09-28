using ServidorChat.controlador;

/// <summary>
/// Variable que nos permite escribir el numero del puerto al momento de ejecutar el programa
/// </summary>
/// <returns></returns>
int puerto = int.Parse(args[0]);

/// <summary>
/// Instancia de la clase ServidorCOntrolador inciada en el puerto aisgnado
/// </summary>
/// <returns></returns>
ServidorControlador servidorControlador = new ServidorControlador(puerto);

do
{   
    await servidorControlador.Iniciar();


} while (servidorControlador.GetServidor().GetActivo());