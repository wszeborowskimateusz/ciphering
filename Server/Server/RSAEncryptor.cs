using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Server
{
    class RSAEncryptor
    {
        private RSACryptoServiceProvider csp;

        public RSAEncryptor(string publicKeyFileName)
        {
            csp = new RSACryptoServiceProvider();

            XmlDocument doc = new XmlDocument();
            doc.Load(publicKeyFileName);
            string xmlcontents = doc.InnerXml;
            csp.FromXmlString(xmlcontents);
            var pubKey = csp.ExportParameters(false);

            csp = new RSACryptoServiceProvider();
            csp.ImportParameters(pubKey);
        }

        public string Encrypt(string messaege)
        {
            var bytesPlainTextData = Encoding.UTF8.GetBytes(messaege);

            var bytesCypherText = csp.Encrypt(bytesPlainTextData, false);

            var cypherText = Convert.ToBase64String(bytesCypherText);

            return cypherText;
        }

        public static void GenerateKeyPair(string publicKeyLocation, string privateKeyLocation, string userName, string userPassword)
        {
            var sha = new SHA256Managed();
            byte[] passwordHash = GenerateHash(userPassword);

            var csp = new RSACryptoServiceProvider(2048);

            var publicKey = csp.ToXmlString(false);
            File.WriteAllText(publicKeyLocation + "\\" + userName + "_public.xml", publicKey);

            var privateKey = csp.ToXmlString(true);

            AESEncryptor encryptor = new AESEncryptor(CipherMode.CBC, AES_SUBBLOCK_SIZE.SUBBLOCK_128, passwordHash);
            var encryptedPrivateKey = Convert.ToBase64String(encryptor.Encrypt(privateKey));

            encryptedPrivateKey = Convert.ToBase64String(encryptor.IV) + "<END_OF_IV>" + encryptedPrivateKey;

            File.WriteAllText(privateKeyLocation + "\\" + userName + "_private.xml", encryptedPrivateKey);
        }

        private static byte[] GenerateHash(string data)
        {
            var crypt = new SHA256Managed();
            var hash = new StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(data));

            return crypto;
        }
    }
}
