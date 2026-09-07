#include <netinet/in.h>

int crearSocket();
int enlaceSocket(int sock, struct sockaddr_in *servidor);
int listenSocket(int sock);
int acceptCliente(int sock, struct sockaddr_in *cliente);
