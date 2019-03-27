using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class TCPServer
    {
        private Int32 ServerPort = 13529;
        private TcpListener server;

        public TCPServer()
        {
            server = new TcpListener(IPAddress.Any, ServerPort);
        }

        public void StartListening()
        {
            server.Start();

            while (true)
            {
                TcpClient client = server.AcceptTcpClient();

                NetworkStream stream = client.GetStream();

                ProcessClient(stream);

                client.Close();
            }
        }

        private void ProcessClient(NetworkStream networkStream)
        {
            
        }
    }
}
