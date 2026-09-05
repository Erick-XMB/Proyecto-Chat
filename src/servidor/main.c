#include <stdio.h>
#include <stdlib.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <unistd.h>
#include "servidor.h"
#include <string.h>

int main()
{

    int puerto = 5022;
    int sock, sock2;
    socklen_t longitud_cliente;

    struct sockaddr_in server;
    struct sockaddr_in client;

    // configurar

    server.sin_family = AF_INET;
    server.sin_port = htons(puerto);
    server.sin_addr.s_addr = INADDR_ANY;

    memset(server.sin_zero, 0, sizeof(server.sin_zero));

    sock = socket(AF_INET, SOCK_STREAM, 0);

    if (sock == -1)
    {
        perror("accept");
        return 1;
    }

    if (bind(sock, (struct sockaddr *)&server, sizeof(server)) == -1)
    {
        perror("en bind");
        return 1;
    }

    if (listen(sock, 5) == -1)
    {
        perror("en listen");
        return 1;
    }

    while (1)
    {
        longitud_cliente = sizeof(struct sockaddr_in);

        sock2 = accept(sock, (struct sockaddr *)&client, &longitud_cliente);

        if (sock2 == -1)
        {
            perror("accept");
            continue;
        }

        printf("Cliente conectado \n");

        char buffer[1024];

        int bytes_recibidos = recv(sock2, buffer, sizeof(buffer) - 1, 0);

        if(bytes_recibidos > 0){
            buffer[bytes_recibidos] = '\0';

            printf("Mensaje recibido: %s\n", buffer);
        }

        close(sock2);
    }

    close(sock);

    return 0;
}