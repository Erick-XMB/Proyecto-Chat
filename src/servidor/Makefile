CC = gcc

CFLAGS = -Wall -Wextra -std=c11

all: servidor
	./servidor

servidor: main.o servidor.o
	$(CC) $(CFLAGS) main.o servidor.o -o servidor

main.o: main.c
	$(CC) $(CFLAGS) -c main.c -o main.o

servidor.o: servidor.c servidor.h
	$(CC) $(CFLAGS) -c servidor.c -o servidor.o


clean:
	rm -f *.o servidor