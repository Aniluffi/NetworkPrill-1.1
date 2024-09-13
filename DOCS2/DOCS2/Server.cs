using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DOCS2
{
    class Server
    {
        List<Socket> clients = new List<Socket>();


        public async Task ServerOn()
        {
            using var tcpLisener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                tcpLisener.Bind(new IPEndPoint(IPAddress.Any, 808));
                tcpLisener.Listen();    // запускаем сервер
                Console.WriteLine("Сервер запущен. Ожидание подключений... ");

                while (true)
                {
                    var tcpClient = await tcpLisener.AcceptAsync();
                    lock (clients)
                    {
                        clients.Add(tcpClient); // Добавление клиента в список
                    }
                    Console.WriteLine($"{tcpClient.RemoteEndPoint} подключился");


                    await Task.Run(async () => Client(tcpClient));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private async Task Client(Socket tcpClient)
        {
            var buffer = new byte[1024];

            try
            {
                while (true)
                {
                    var bytesRead = await tcpClient.ReceiveAsync(buffer);

                    var message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"Сообщение от {tcpClient.RemoteEndPoint}: {message}");

                    await BroadCast(tcpClient, message);
                }
            }
            catch (SocketException)
            {
                Console.WriteLine($"Клиент {tcpClient.RemoteEndPoint} отключился");
            }

        }

        private async Task BroadCast(Socket tcpClient, string message)
        {
            try
            {
                foreach (var client in clients)
                {
                    var mess = Encoding.UTF8.GetBytes($"\n Сообщение от {tcpClient.RemoteEndPoint}:  " + message);
                    if (tcpClient.RemoteEndPoint != client.RemoteEndPoint)
                        await client.SendAsync(mess);
                }
            }
            catch (SocketException)
            {
                Console.WriteLine("Ошибка рассылки");
            }

        }
        }
    }
