using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Client
{
    class TCPServer
    {
        private Int32 ServerPort = 13529;
        private TcpListener server;
        private string outputFileLocation = "C:\\Users\\Mateusz\\Desktop\\BSK_projekty\\Ciphering\\OutputFiles";

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

        private void ProcessClient(NetworkStream stream)
        {
            string fileName = "";
            string fileHeader = "";
            using (StreamReader reader = new StreamReader(stream))
            {
                fileName = reader.ReadLine();
                fileHeader = reader.ReadLine();
            }

            fileName = outputFileLocation + "\\" + fileName;

            using (StreamWriter sw = File.CreateText(fileName))
            {
                sw.WriteLine(fileHeader);
            }

            using (BinaryWriter writer = new BinaryWriter(File.Open(fileName, FileMode.Open)))
            {
                int numBytesRead;
                while ((numBytesRead = stream.Read(data, 0, data.Length)) > 0)
                {
                    writer.Write(data, 0, numBytesRead);
                }               
            }

            stream.Close();
        }
    }
}
