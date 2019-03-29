using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Server
{
    class User
    {
        public string Name { get; set; }
        public string SessionKey { get; set; }
    }

    class FileSender
    {
        private RSAEncryptor rsaEncryptor;
        private AESEncryptor aesEncryptor;
        private User[] listOfUsersToSend;

        public FileSender(RSAEncryptor re, AESEncryptor ae, User[] users)
        {
            rsaEncryptor = re;
            aesEncryptor = ae;
            listOfUsersToSend = users;
        }



        public string GenerateXmlHeader(User[] listOfUsers, string fileExtension, string keyLengthVal, string cypherModeVal, string subBlockLengthVal)
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

                XmlText keyLengthText = doc.CreateTextNode(rsaEncryptor.Encrypt(keyLengthVal));
                XmlText sessionKeyText = doc.CreateTextNode(rsaEncryptor.Encrypt(user.SessionKey));
                XmlText cypherModeText = doc.CreateTextNode(rsaEncryptor.Encrypt(cypherModeVal));
                XmlText subBlockLengthText = doc.CreateTextNode(rsaEncryptor.Encrypt(subBlockLengthVal));

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
