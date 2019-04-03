using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //RSAEncryptor.GenerateKeyPair("C:\\Users\\Mateusz\\Desktop\\BSK_projekty\\Ciphering\\keys\\Public",
            //    "C:\\Users\\Mateusz\\Desktop\\BSK_projekty\\Ciphering\\keys\\Private", "user1", "alamakota");

            //RSAEncryptor.GenerateKeyPair("C:\\Users\\Mateusz\\Desktop\\BSK_projekty\\Ciphering\\keys\\Public",
            //    "C:\\Users\\Mateusz\\Desktop\\BSK_projekty\\Ciphering\\keys\\Private", "user2", "alamakota");

            //RSAEncryptor.GenerateKeyPair("C:\\Users\\Mateusz\\Desktop\\BSK_projekty\\Ciphering\\keys\\Public",
            //    "C:\\Users\\Mateusz\\Desktop\\BSK_projekty\\Ciphering\\keys\\Private", "user3", "alamakota");

            User[] users = {
                new User() {
                    Name = "user4",
                    rsaEncryptor = new RSAEncryptor("C:\\Users\\Mateusz\\Desktop\\BSK_Projekty\\Ciphering\\keys\\Public\\user4_public.xml")
                },
                new User() {
                    Name = "user5",
                    rsaEncryptor = new RSAEncryptor("C:\\Users\\Mateusz\\Desktop\\BSK_Projekty\\Ciphering\\keys\\Public\\user5_public.xml")
                }
            };

            FileSender fileSender = new FileSender(new AESEncryptor(System.Security.Cryptography.CipherMode.CBC,
                AES_KEY_SIZE.KEY_128, AES_SUBBLOCK_SIZE.SUBBLOCK_128), users);

            fileSender.SendFile("C:\\Users\\Mateusz\\Desktop\\BSK_projekty\\Ciphering\\TestFiles\\video.mp4");

        }
    }
}
