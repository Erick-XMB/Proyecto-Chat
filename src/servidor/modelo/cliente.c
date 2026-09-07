
#include <stdio.h>
#include <stdlib.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <unistd.h>

void recibirCliente(int sockCliente)
{

    /** aqui se guardaran los datos que lleguen */
    char buffer[1024];

    /** Aqui se reciben los dtos que llegaron a traves de la conexion del socket */
    int bytes_recibidos = recv(sockCliente, buffer, sizeof(buffer) - 1, 0);

    /** Si los bytes se guardaron y leyeron con exito en el buffer */
    if (bytes_recibidos > 0)
    {
        buffer[bytes_recibidos] = '\0';

        printf("Mensaje recibido: %s\n", buffer);
    }
}