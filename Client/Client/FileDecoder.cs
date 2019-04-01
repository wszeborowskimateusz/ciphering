using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Client
{
    class FileDecoder
    {
        private string privateKeysLocation = "C:\\Users\\Mateusz\\Desktop\\BSK_projekty\\Ciphering\\keys\\Private";

        public FileDecoder()
        {

        }

        public XmlDocument GetFileHeader(string fileName)
        {
            String header = "";
            using (StreamReader sr = new StreamReader(fileName))
            {
                header = sr.ReadLine();
            }

            var xmlHeader = new XmlDocument();
            xmlHeader.LoadXml(header);

            return xmlHeader;
        }

        public string DecryptPrivateKey(string userName, string password)
        {
            byte[] passwordHash = GenerateHash(password);
            string fileName = privateKeysLocation + "\\" + userName + "_private.xml";

            byte[] IV;

            var lines = File.ReadAllText(fileName);

            var privateKeyElements = lines.Split(new string[] { "<END_OF_IV>" }, StringSplitOptions.None);

            IV = Convert.FromBase64String(privateKeyElements[0]);

            AESDecryptor decryptor = new AESDecryptor(CipherMode.CBC, AES_KEY_SIZE.KEY_256, AES_SUBBLOCK_SIZE.SUBBLOCK_128, passwordHash, IV);

            var decryptedKey = decryptor.Decrypt(Convert.FromBase64String(privateKeyElements[1]));

            var decryptedKeyString = Encoding.UTF8.GetString(decryptedKey);

            return decryptedKeyString;
        }

        private static byte[] GenerateHash(string data)
        {
            var crypt = new SHA256Managed();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(data));

            return crypto;
        }
    }
}
