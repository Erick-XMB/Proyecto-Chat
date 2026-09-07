CC = gcc

CFLAGS = -Wall -Wextra -std=c11

ARCHIVOS = main.o \
	controlador/servidorControlador.o \
	modelo/socket.o \
	modelo/cliente.o
 

all: servidor
	./servidor

servidor: main.o $(ARCHIVOS)
	$(CC) $(CFLAGS) $(ARCHIVOS) -o servidor

main.o: main.c
	$(CC) $(CFLAGS) -c main.c -o main.o

servidorControlador.o: controlador/servidorControlador.c controlador/servidorControlador.h
	$(CC) $(CFLAGS) -c controlador/servidorControlador.c -o controlador/servidorControlador.o

modelo/socket.o: modelo/socket.c modelo/socket.h
	$(CC) $(CFLAGS) -c modelo/socket.c -o modelo/socket.o

modelo/cliente.o: modelo/cliente.c modelo/cliente.h
	$(CC) $(CFLAGS) -c modelo/cliente.c -o modelo/cliente.o


clean:
	find . -name "*.o" -delete
	rm -f *.o servidor