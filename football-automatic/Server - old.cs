using SocketIOSharp.Common.Packet;
using SocketIOSharp.Server;
using SocketIOSharp.Server.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace football_automatic
{
    internal class Server_old
    {
        private SocketIOServer _server;

        public Server_old() 
        {
            _server = new SocketIOServer(new SocketIOServerOption(80, AllowEIO3:true));
            _server.OnConnection(OnConnection);
        }

        public void Start() => _server.Start();

        private void OnConnection(SocketIOSocket socket)
        {
            //Console.WriteLine($"{socket.GetHashCode} has connected!");

            /*socket.On("message", (SocketIOAckEvent ack) =>
            {
                Console.WriteLine("Received message: " + ack.Data);
                // Process the received message from the client
            });*/

            socket.Emit("socketio-client", "Emit");
            //socket.On("test", OnTest);
        }

        private void OnTest(SocketIOAckEvent ack) { }
    }
}
