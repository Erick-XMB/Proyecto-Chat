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

documentacion:
	cd documentos/documentacion && docfx
	
verDocumentacion:
	cd documentos/documentacion && docfx serve _site

clean:
	dotnet clean src/servidor/ServidorChat.csproj
	dotnet clean src/cliente/ClienteChat.csproj
	dotnet clean src/protocoloMensajes/protocoloMensajes.csproj
	verDocumentacion:
	cd documentos/documentacion && docfx serve _site