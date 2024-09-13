using System.Net.Sockets;
using System.Text;


class Program
{
    public static async Task Main()
    {
        using var tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            await tcpClient.ConnectAsync("127.0.0.1", 808);


            while (true)
            {
                Console.WriteLine("Send:");
                string word = Console.ReadLine();
                byte[] data = Encoding.UTF8.GetBytes(word);

                await tcpClient.SendAsync(data);

                var buf = new byte[1024];
                int acceptBytes = await tcpClient.ReceiveAsync(buf);

                string acceptWord = Encoding.UTF8.GetString(buf, 0, acceptBytes);
                Console.WriteLine($"{acceptWord}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

    }
}
