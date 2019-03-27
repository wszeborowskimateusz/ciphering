using System;
using System.Collections.Generic;
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
            var bytesPlainTextData = System.Text.Encoding.Unicode.GetBytes(messaege);

            var bytesCypherText = csp.Encrypt(bytesPlainTextData, false);

            var cypherText = Convert.ToBase64String(bytesCypherText);

            return cypherText;
        }
    }
}
