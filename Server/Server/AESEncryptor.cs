using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    //OFB mode doesn't work
    class AESEncryptor
    {
        private byte[] key;
        private byte[] IV;
        private RijndaelManaged Aes;
        private CipherMode mode;
        private AES_KEY_SIZE keySize;


        public AESEncryptor(CipherMode m, AES_KEY_SIZE kS)
        {
            Aes = new RijndaelManaged();
            Aes.FeedbackSize = 8;
            Aes.Padding = PaddingMode.PKCS7;
            Aes.Mode = m;
            Aes.BlockSize = 128;
            Aes.KeySize = (int)kS;
            Aes.GenerateKey();
            Aes.GenerateIV();

            key = new byte[((int)kS) / 8];
            IV = new byte[((int)kS) / 8];

            Aes.Key.CopyTo(key, 0);
            Aes.IV.CopyTo(IV, 0);

            mode = m;
            keySize = kS;
        }

        ~AESEncryptor()
        {
            Aes.Clear();
        }

        public byte[] Encrypt(String Plain)
        {
            byte[] plainBytes = Encoding.UTF8.GetBytes(Plain);
            byte[] cipherBytes;

            using (var encryptor = Aes.CreateEncryptor())
            using (var msEncrypt = new MemoryStream())
            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            using (var bw = new BinaryWriter(csEncrypt, Encoding.UTF8))
            {
                bw.Write(plainBytes);
                bw.Close();

                cipherBytes = msEncrypt.ToArray();

                //Console.WriteLine("Cipher text is " + BitConverter.ToString(cipherBytes));
            }

            return cipherBytes;
        }
    }
}
