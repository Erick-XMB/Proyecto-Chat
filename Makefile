all: servidor

servidor: 
	$(MAKE) -C src/servidor all

clean:
	$(MAKE) -C src/servidor clean