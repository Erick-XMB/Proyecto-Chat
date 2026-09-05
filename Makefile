all: servidor cliente

servidor: 
	$(MAKE) -C src/servidor all

cliente:
	dotnet build src/cliente/ClienteChat.csproj

runCliente:
	dotnet run --project src/cliente/ClienteChat.csproj

clean:
	$(MAKE) -C src/servidor clean
	dotnet clean src/cliente/ClienteChat.csproj