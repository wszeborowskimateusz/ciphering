using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
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
            xmlString = Regex.Replace(xmlString, @"[^\u0000-\u007F]+", string.Empty);
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.LoadXml(xmlString);
                string xmlcontents = doc.InnerXml;
                csp.FromXmlString(xmlcontents);

                var privKey = csp.ExportParameters(true);


                csp.ImportParameters(privKey);
            }
            catch (Exception e)
            {
                csp = new RSACryptoServiceProvider(2048);
            }
            
            
            
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
