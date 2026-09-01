CC = gcc

CFLAGS = -Wall -Wextra -std=c11

all: servidor
	./servidor

servidor: holaMundo.o
	$(CC) $(CFLAGS) holaMundo.o -o servidor

holaMundo.o : rooms/holaMundo.c
	$(CC) $(CFLAGS) -Irooms -c rooms/holaMundo.c -o holaMundo.o

clean:
	rm -f *.o servidor