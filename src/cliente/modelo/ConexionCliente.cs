using System;
using System.Net.Sockets;
using System.Text;

namespace Cliente.modelo;

public class ConexionCliente
{

    /** Checa el estado de la conexion TCP*/
    private TcpClient cliente;

    /** Atributo que maneja el flujo de la lectura*/
    private NetworkStream stream;


    /// <summary>
    /// Metodo que nos permite conectarnos al servidor
    /// </summary>
    /// <returns>
    /// <c>true</c> si se pudo establecer conexion, en caso contrario, <c>false</c>.
    /// </returns>
    public bool Conectar()
    {   
        /** Esta es la drieccion de servdior, usamos la del localhost*/
        string servidor = "127.0.0.1";

        /** Este el numero del puerto que estara esperando conexiones*/
        int puerto = 5022;

        try
        {   
            /** CReamos un objeto TcpClient con el servidor y puerto previamente definidos*/
            cliente = new TcpClient(servidor, puerto);

            /** Este es el canal de comunicacion asociado al socket TCP*/
            stream = cliente.GetStream();

            return true;

        }
        catch (Exception)
        {
            return false;
        }

    }

    /// <summary>
    /// Metodo que envia un mensaje al servidor usando la conexion de TCP
    /// </summary>
    /// <param name="mensaje">
    /// Cadena de texto que se quiere enviar al servidor
    /// </param>
    public void EnviarMensaje(string mensaje)
    {   
        /** Aqui se verifica que haya una conexionn establecida con el servidor
            pues si es null siginifica que todavia no hay conexion*/
        if (stream == null)
        {
            return;
        }

        /** Aqui se conierten una cadena de texto en un arreglos de numero binarios
            con el formato UTF8*/
        byte[] datos = Encoding.UTF8.GetBytes(mensaje);

        /** Enviamos los bytes que tenemos en datos usando el NetWorkStream*/
        stream.Write(datos, 0, datos.Length);


    }



}


