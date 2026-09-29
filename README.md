Proyecto Chat MyP
========================
Proyecto número 1 de la materia modelado y programación, que implementa un Chat en donde se involucra el tema de conexión de red usando 
el protocolo TCP.

Comandos sistemas de construccion
-------------

Los comandos para compilar y ejecutar el servidor son:

* make servidor
* make runServidor p={el numero del puerto con el que deseamos iniciar el servidor}


Los comandos para compilar y ejecutar el cliente son:

* make cliente
* make runCliente

Comandos extra para la limpieza de archivos
* make limpiarServidor
* make limpiarCliente
* make clean

Comando general
* make all (solo compila tanto el cliente como el servidor)

Documentación
-------------------
La documentación del proyecto se genera mediante DocFX 

Para generar la documentación:
* make documentacion

Despues de ejecutar dicho comando, se podrá consultar la documentación generada con:

* make verDocumentacion


Teconologias utilizadas en el proyecto
-------------------
- .NET SDK 10.0.112
- System.Text.Json (Incluido en .NET)
- Framework: Avalonia 12.1.1
- DocFX 2.80.1 para la documentacion
- Make 4.3 como sistema de construcción


Desarrollador
---------
* Erick Xavier Martinez Briones
* No cuenta = 426007742
* Correo electronico = erick.martinezkmn@ciencias.unam.mx