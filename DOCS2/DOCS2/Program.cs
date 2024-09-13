using DOCS2;
using System.Net;
using System.Net.Sockets;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

class Program
{

    public static async Task Main()
    {

        Server server = new Server();
        await server.ServerOn();
    }

  
}
