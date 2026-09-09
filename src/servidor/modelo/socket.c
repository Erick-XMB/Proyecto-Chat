#include <stdio.h>
#include <stdlib.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <unistd.h>
#include "socket.h"
#include <string.h>

int crearSocket()
{
    /** llamamos a nuestro socket sock, con los parametros
     * AF_INET usamos ara IPv4
     * SOCK_STREAM pues estamos trabajando con TCP
     * 0, porque asi me marco el tutorial xd
     */
    int sock = socket(AF_INET, SOCK_STREAM, 0);

    if (sock == -1)
    {
        perror("Socket");
    }

    return sock;
}

int enlaceSocket(int sock, struct sockaddr_in *servidor)
{
    /** Llamamos a bind para hacer el enlace
     * sock es numero de identificacion que se nos dio al llamar a la funcion crearSocket
     * servidor es donde estamos guardando los datos como el puerto
     * el tercer parametro es el tamanio de la estruuctura que usamos en el segundo parametro
     *
     */
    if (bind(sock, (struct sockaddr *)servidor, sizeof(*servidor)) == -1)
    {
        perror("en bind aquiiiiiiii");
        return -1;
    }
    return 0;
}

int listenSocket(int sock)
{
    /** usamos el listen para que se prepara para aceptar las conexiones
     * sock pues el socket que estamos usando
     * el 5 nada mas son las conexiones que estan esperando
     */
    if (listen(sock, 5) == -1)
    {
        perror("en listen");
        return -1;
    }

    return 0;
}

int acceptCliente(int sock, struct sockaddr_in *cliente)
{

    /** aqui es donde se guardar el tamanio donde se guarda la informacion del cliente */
    socklen_t longitudCLiente = sizeof(struct sockaddr_in);

    /** usamos el accpet para aceptar la conexion del listen
     * sock es el socket que estamos usando
     * cliente es donde estara la informacion de red
     * longitudCliente es el tamanio de la estructura del cliente
     *
     */
    int sockCliente = accept(sock, (struct sockaddr *)cliente, &longitudCLiente);

    if (sockCliente == -1)
    {
        perror("accept");
    }

    return sockCliente;
}

int recibirMensaje(int sockCliente, char *buffer, int tamanio)
{

    int bytes_recibidos;

    /** Aqui se reciben los dtos que llegaron a traves de la conexion del socket */
    bytes_recibidos = recv(sockCliente, buffer, sizeof(buffer) - 1, 0);

    /** si hay algun fallo devolvemos el resultado */
    if (bytes_recibidos <= 0)
    {
        return bytes_recibidos;
    }

    /** agregamos el carcater /0  para que se trate como una cadena*/
    buffer [bytes_recibidos] = '\0';

    /** regresamos los bytes  */
    return bytes_recibidos;
}
