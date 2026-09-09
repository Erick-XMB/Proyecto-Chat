#include "servidorControlador.h"
#include "../modelo/socket.h"
#include "../modelo/cliente.h"

#include <netinet/in.h>
#include <unistd.h>
#include <string.h>
#include <stdio.h>

#define PORT 5041

int iniciarServidor()
{
    struct sockaddr_in servidor;
    struct sockaddr_in cliente;

    servidor.sin_family = AF_INET;
    servidor.sin_port = htons(PORT);
    servidor.sin_addr.s_addr = INADDR_ANY;

    /** usamos memset para limpiar la estructura "servidor" antes de usarla */
    memset(servidor.sin_zero, 0, sizeof(servidor.sin_zero));

    /** creamos nuestro socket */
    int sock = crearSocket();

    if (sock == -1)
    {
        return 1;
    }

    /** enlazamos nuestro socket */
    if (enlaceSocket(sock, &servidor))
    {
        return 1;
    }

    /** lo ponemos en espera */
    if (listenSocket(sock))
    {
        return 1;
    }

    while (1)
    {
        /** aceptamos al cliente */
        int sockCliente = acceptCliente(sock, &cliente);

        if (sockCliente == -1)
        {
            continue;
        }

        char buffer[1024];

        int bytes = recibirMensaje(sockCliente, buffer, sizeof(buffer));

        if(bytes > 0)
        {
            printf("Mensaje recibido: %s\n",  buffer);
        }
        

        close(sockCliente);
    }

    close(sock);

    return 0;
}