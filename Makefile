all: servidor cliente 

servidor: 
	dotnet build src/servidor/ServidorChat.csproj

runServidor: 
	dotnet run --project src/servidor/ServidorChat.csproj -- $(p)

limpiarServidor:
	dotnet clean src/servidor/ServidorChat.csproj

cliente:
	dotnet build src/cliente/ClienteChat.csproj

runCliente:
	dotnet run --project src/cliente/ClienteChat.csproj

limpiarCliente:
	dotnet clean src/cliente/ClienteChat.csproj

clean:
	dotnet clean src/servidor/ServidorChat.csproj
	dotnet clean src/cliente/ClienteChat.csproj
	