using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace football_automatic
{
    internal class Server
    {
        private TcpListener _server;

        public Server()
        {
            _server = new TcpListener(IPAddress.Parse("127.0.0.1"), 3000);

            Console.WriteLine($"Responding to: {_server.LocalEndpoint}");
        }

        public void Start() => _server.Start();

        private async Task StartListening()
        {
            while (true)
            {
                TcpClient client = await _server.AcceptTcpClientAsync();
                Task.Run(() => HandleClientAsync(client));
            }
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[client.ReceiveBufferSize];

            while (true)
            {
                int bytesRead = await stream.ReadAsync(buffer, 0, client.ReceiveBufferSize);
                if (bytesRead == 0)
                {
                    // Client disconnected
                    break;
                }

                string requestData = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                // Process requestData and perform necessary operations

                // Send a response if required
                byte[] responseBuffer = Encoding.ASCII.GetBytes("Response to client");
                await stream.WriteAsync(responseBuffer, 0, responseBuffer.Length);
            }

            client.Close();
        }
    }
}
