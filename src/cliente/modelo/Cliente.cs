using System.Net;
using System.Net.Sockets;

public class Cliente
{
    private string username;

    private string status;

    private readonly TcpClient socketCliente;

    public Cliente(string username, string status, TcpClient socketCliente)
    {
        this.username = username;
        this.status = status;
        this.socketCliente = socketCliente;
    }

}