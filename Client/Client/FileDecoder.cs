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

        public void DecryptFile(string fileName, string targetFile, string userName, string password)
        {
            XmlDocument fileHeader = GetFileHeader(fileName);
            string fileExtension = GetFileExtensionFromFileHeader(fileHeader);
            AESDecryptor decryptor = GetAesFromFileHeader(fileHeader, userName, password);

            DecryptFile(fileName, targetFile, fileExtension, decryptor);
        }

        public void DecryptFile(string fileName, string targetFile, string fileExtension, AESDecryptor decryptor)
        {
            if (File.Exists(fileName))
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    //skip header
                    sr.ReadLine();
                    sr.Close();
                    using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
                    {
                        byte[] data = new byte[1024];

                        using (BinaryWriter writer = new BinaryWriter(File.Open(targetFile + fileExtension, FileMode.Create)))
                        {
                            int numBytesRead;
                            while ((numBytesRead = reader.Read(data, 0, data.Length)) > 0)
                            {
                                byte[] decryptedData = decryptor.Decrypt(data);

                                writer.Write(decryptedData, 0, decryptedData.Length);
                            }
                        }

                    }
                }
            }
        }

        public string[] GetListOfUsersFromFileHeader(XmlDocument fileHeader)
        {
            var users = fileHeader.ChildNodes.Item(0).ChildNodes.Item(0);

            string[] result = new string[users.ChildNodes.Count]; 

            for(int i = 0; i < users.ChildNodes.Count; i++)
            {
                result[i] = users.ChildNodes.Item(i).InnerText;
            }

            return result;
        }

        public string GetFileExtensionFromFileHeader(XmlDocument fileHeader)
        {
            return fileHeader.ChildNodes.Item(0).Attributes.Item(0).InnerText;
        }

        public AESDecryptor GetAesFromFileHeader(XmlDocument fileHeader, string userName, string password)
        {
            var users = GetListOfUsersFromFileHeader(fileHeader);

            int user_id = 0;

            for(int i = 0; i < users.Length; i++)
            {
                if(users[i].CompareTo(userName) == 0)
                {
                    user_id = i;
                    break;
                }
            }

            var userInfo = fileHeader.ChildNodes.Item(0).ChildNodes.Item(user_id + 1);

            var encryptedKeyLength = userInfo.ChildNodes.Item(0).InnerText;
            var encryptedSessionKey = userInfo.ChildNodes.Item(1).InnerText;
            var encryptedCypherMode = userInfo.ChildNodes.Item(2).InnerText;
            var encryptedSubBlockLength = userInfo.ChildNodes.Item(3).InnerText;
            var encryptedIV = userInfo.ChildNodes.Item(4).InnerText;

            var privateKey = DecryptPrivateKey(userName, password);

            RSADecryptor decryptor = new RSADecryptor(privateKey);

            var keyLength = decryptor.Decrypt(encryptedKeyLength);
            var sessionKey = decryptor.Decrypt(encryptedSessionKey);
            var cypherMode = decryptor.Decrypt(encryptedCypherMode);
            var subBlockLength = decryptor.Decrypt(encryptedSubBlockLength);
            var IV = decryptor.Decrypt(encryptedIV);

            CipherMode cipherMode = CipherMode.ECB;
            switch(cypherMode)
            {
                case "ECB":
                    cipherMode = CipherMode.ECB;
                    break;
                case "CBC":
                    cipherMode = CipherMode.CBC;
                    break;
                case "CFB":
                    cipherMode = CipherMode.CFB;
                    break;
                case "OFB":
                    cipherMode = CipherMode.OFB;
                    break;
            }

            Console.WriteLine("keyLength: {0}\nsessionKey: {1}\ncypherMode: {2}\nsubBlock: {3}\nIV: {4}", keyLength, sessionKey, cypherMode, subBlockLength, IV);

            var keyBytes = Convert.FromBase64String(sessionKey);
            var IVBytes = Convert.FromBase64String(IV);

            AESDecryptor aesDecryptor = new AESDecryptor(cipherMode, ConvertNumberToKeySize(keyLength), ConvertNumberToSubBlockSize(keyLength),
                Convert.FromBase64String(sessionKey), Convert.FromBase64String(IV));

            return aesDecryptor;

        } 

        private AES_KEY_SIZE ConvertNumberToKeySize(string keySize)
        {
            switch(keySize)
            {
                case "128":
                    return AES_KEY_SIZE.KEY_128;
                case "192":
                    return AES_KEY_SIZE.KEY_192;
                case "256":
                    return AES_KEY_SIZE.KEY_256;
            }

            return AES_KEY_SIZE.KEY_128;
        }

        private AES_SUBBLOCK_SIZE ConvertNumberToSubBlockSize(string keySize)
        {
            switch (keySize)
            {
                case "8":
                    return AES_SUBBLOCK_SIZE.SUBBLOCK_8;
                case "16":
                    return AES_SUBBLOCK_SIZE.SUBBLOCK_16;
                case "24":
                    return AES_SUBBLOCK_SIZE.SUBBLOCK_24;
                case "32":
                    return AES_SUBBLOCK_SIZE.SUBBLOCK_32;
                case "40":
                    return AES_SUBBLOCK_SIZE.SUBBLOCK_40;
                case "48":
                    return AES_SUBBLOCK_SIZE.SUBBLOCK_48;
                case "56":
                    return AES_SUBBLOCK_SIZE.SUBBLOCK_56;
                case "64":
                    return AES_SUBBLOCK_SIZE.SUBBLOCK_64;
                case "72":
                    return AES_SUBBLOCK_SIZE.SUBBLOCK_72;
                case "80":
                    return AES_SUBBLOCK_SIZE.SUBBLOCK_80;
                case "88":
                    return AES_SUBBLOCK_SIZE.SUBBLOCK_88;
                case "96":
                    return AES_SUBBLOCK_SIZE.SUBBLOCK_96;
                case "104":
                    return AES_SUBBLOCK_SIZE.SUBBLOCK_104;
                case "112":
                    return AES_SUBBLOCK_SIZE.SUBBLOCK_112;
                case "120":
                    return AES_SUBBLOCK_SIZE.SUBBLOCK_120;
                case "128":
                    return AES_SUBBLOCK_SIZE.SUBBLOCK_128;
            }

            return AES_SUBBLOCK_SIZE.SUBBLOCK_128;
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
