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
        public byte[] IV { get; }
        public RijndaelManaged Aes { get; }
        public CipherMode mode { get; }
        public AES_KEY_SIZE keySize { get; }
        public AES_SUBBLOCK_SIZE subBlockSize { get; }


        public AESEncryptor(CipherMode m, AES_KEY_SIZE kS, AES_SUBBLOCK_SIZE sS)
        {
            Aes = new RijndaelManaged();
            //I guess this is subblock size
            Aes.FeedbackSize = (int)sS;
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
            subBlockSize = sS;
        }

        //This is for ciphering clients private key
        public AESEncryptor(CipherMode m,  AES_SUBBLOCK_SIZE sS, byte[] keyVal)
        {

            Aes = new RijndaelManaged();
            //I guess this is subblock size
            Aes.FeedbackSize = (int)sS;
            Aes.Padding = PaddingMode.PKCS7;

            Aes.Mode = m;
            Aes.BlockSize = 128;
            Aes.KeySize = 256;
            
            Aes.Key = keyVal;
            Aes.GenerateIV();

            key = new byte[256 / 8];
            IV = new byte[256 / 8];

            Aes.Key.CopyTo(key, 0);
            Aes.IV.CopyTo(IV, 0);

            mode = m;
            keySize = AES_KEY_SIZE.KEY_256;
            subBlockSize = sS;
        }

        ~AESEncryptor()
        {
            Aes.Clear();
        }

        public byte[] Encrypt(String Plain, byte[] byteArray = null)
        {
            byte[] plainBytes;
            if (byteArray == null)
                plainBytes = Encoding.UTF8.GetBytes(Plain);
            else
                plainBytes = byteArray;

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
