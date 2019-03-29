﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Server
{
    class FileSender
    {
        private AESEncryptor aesEncryptor;
        private User[] listOfUsersToSend;
        private string ServerAddress = "localhost";
        private Int32 ServerPort = 13529;
        private TcpClient client;

        public FileSender(AESEncryptor ae, User[] users)
        {
            aesEncryptor = ae;
            listOfUsersToSend = users;
        }

        public void SendFile(string fileName, string keyLengthVal, string cypherModeVal, string subBlockLengthVal)
        {
            client = new TcpClient(ServerAddress, ServerPort);

            using (NetworkStream stream = client.GetStream())
            {

                if (File.Exists(fileName))
                {
                    using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
                    {
                        byte[] data = new byte[1024];

                        string fileExtension = Path.GetExtension(fileName);

                        string fileHeader = GenerateXmlHeader(listOfUsersToSend, fileExtension,
                            ((int)aesEncryptor.keySize).ToString(), aesEncryptor.mode.ToString(),
                            ((int)aesEncryptor.subBlockSize).ToString(), BitConverter.ToString(aesEncryptor.Aes.Key));

                        using (StreamWriter writer = new StreamWriter(stream))
                        {
                            writer.WriteLine(fileName);
                            writer.Flush();

                            writer.WriteLine(fileHeader);
                            writer.Flush();
                        }

                        int numBytesRead;
                        while ((numBytesRead = reader.Read(data, 0, data.Length)) > 0)
                        {
                            byte[] cipheredData = aesEncryptor.Encrypt(null, data);

                            stream.Write(cipheredData, 0, cipheredData.Length);
                        }

                    }
                }
            }
        }

        public string GenerateXmlHeader(User[] listOfUsers, string fileExtension, string keyLengthVal, string cypherModeVal, string subBlockLengthVal, string sessionKeyVal)
        {
            XmlDocument doc = new XmlDocument();


            XmlElement fileHeader = doc.CreateElement("fileHeader");
            fileHeader.SetAttribute("extension", fileExtension);
            doc.AppendChild(fileHeader);

            XmlElement users = doc.CreateElement("users");
            fileHeader.AppendChild(users);

            foreach (User user in listOfUsers)
            {
                XmlElement xmlUser = doc.CreateElement("user");
                XmlText userName = doc.CreateTextNode(user.Name);
                xmlUser.AppendChild(userName);
                users.AppendChild(xmlUser);
            }

            foreach (User user in listOfUsers)
            {
                XmlElement userInfo = doc.CreateElement("userInfo");

                XmlElement keyLength = doc.CreateElement("keyLength");
                XmlElement sessionKey = doc.CreateElement("sessionKey");
                XmlElement cypherMode = doc.CreateElement("cypherMode");
                XmlElement subBlockLength = doc.CreateElement("subBlockLength");

                XmlText keyLengthText = doc.CreateTextNode(user.rsaEncryptor.Encrypt(keyLengthVal));
                XmlText sessionKeyText = doc.CreateTextNode(user.rsaEncryptor.Encrypt(sessionKeyVal));
                XmlText cypherModeText = doc.CreateTextNode(user.rsaEncryptor.Encrypt(cypherModeVal));
                XmlText subBlockLengthText = doc.CreateTextNode(user.rsaEncryptor.Encrypt(subBlockLengthVal));

                keyLength.AppendChild(keyLengthText);
                sessionKey.AppendChild(sessionKeyText);
                cypherMode.AppendChild(cypherModeText);
                subBlockLength.AppendChild(subBlockLengthText);

                userInfo.AppendChild(keyLength);
                userInfo.AppendChild(sessionKey);
                userInfo.AppendChild(cypherMode);
                userInfo.AppendChild(subBlockLength);

                fileHeader.AppendChild(userInfo);
            }

            return doc.OuterXml;
        }
    }
}
