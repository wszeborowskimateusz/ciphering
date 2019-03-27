using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
   
    class TCPClient
    {
        private TcpClient client;
        private string ServerAddress = "localhost";
        private Int32 ServerPort = 13529;

        public void SendFile(string message)
        {
            client = new TcpClient(ServerAddress, ServerPort);

            using (NetworkStream stream = client.GetStream())
            {
                Byte[] data = Encoding.ASCII.GetBytes(message);
                stream.Write(data, 0, data.Length);
            };
        }
    }
}
