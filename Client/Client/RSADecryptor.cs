using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Client
{
    class RSADecryptor
    {
        private RSACryptoServiceProvider csp;

        public RSADecryptor(string xmlString)
        {
            csp = new RSACryptoServiceProvider();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlString);
            string xmlcontents = doc.InnerXml;
            csp.FromXmlString(xmlcontents);
            var privKey = csp.ExportParameters(true);


            csp.ImportParameters(privKey);
        }

        public string Decrypt(string message)
        {
            var bytesCypherText = Convert.FromBase64String(message);
            string plainTextData = "";
            try
            {
                var bytesPlainTextData = csp.Decrypt(bytesCypherText, false);
                plainTextData = Encoding.UTF8.GetString(bytesPlainTextData);
            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e.Message);
            }

            return plainTextData;
        }
    }
}
