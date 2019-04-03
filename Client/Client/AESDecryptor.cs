using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class AESDecryptor
    {
        private byte[] key;
        private byte[] IV;
        public RijndaelManaged Aes;
        private CipherMode mode;
        private AES_KEY_SIZE keySize;
        private AES_SUBBLOCK_SIZE subBlockSize;

        public AESDecryptor(CipherMode m, AES_KEY_SIZE kS, AES_SUBBLOCK_SIZE sS, byte[] symmetricKey, byte[] suppliedIV)
        {
            Aes = new RijndaelManaged();

            Aes.FeedbackSize = (int)sS;
            Aes.Padding = PaddingMode.Zeros;

            Aes.Mode = m;
            Aes.BlockSize = 128;
            Aes.KeySize = (int)kS;
            Aes.Key = symmetricKey;
            Aes.IV = suppliedIV;

            key = new byte[((int)kS) / 8];
            IV = new byte[Aes.BlockSize / 8];

            Aes.Key.CopyTo(key, 0);
            Aes.IV.CopyTo(IV, 0);

            mode = m;
            keySize = kS;
            subBlockSize = sS;
        }

        public byte[] Decrypt(byte[] cipherBytes)
        {
            byte[] plainBytes;
            int count = 0;
            using (var decryptor = Aes.CreateDecryptor(Aes.Key, Aes.IV))
            {
                using (var msEncrypt = new MemoryStream(cipherBytes))
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, decryptor, CryptoStreamMode.Read))
                    {
                        plainBytes = new byte[cipherBytes.Length];
                        
                        count = csEncrypt.Read(plainBytes, 0, plainBytes.Length);

                    }
                }
            }

            byte[] returnval = new byte[count];
            Array.Copy(plainBytes, returnval, count);
            return returnval;
        }

    }
}
