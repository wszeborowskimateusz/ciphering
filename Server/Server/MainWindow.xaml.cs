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

            User[] users = {
                new User() {
                    Name = "client1",
                    rsaEncryptor = new RSAEncryptor("C:\\Users\\Mateusz\\Desktop\\BSK_Testy\\keys\\Public\\client1_public.xml")
                },
                new User() {
                    Name = "client2",
                    rsaEncryptor = new RSAEncryptor("C:\\Users\\Mateusz\\Desktop\\BSK_Testy\\keys\\Public\\client2_public.xml")
                }
            };

            FileSender fileSender = new FileSender(new AESEncryptor(System.Security.Cryptography.CipherMode.CBC, 
                AES_KEY_SIZE.KEY_128, AES_SUBBLOCK_SIZE.SUBBLOCK_128), users);

            fileSender.SendFile("C:\\Users\\Mateusz\\Desktop\\Screeny V2\\Przechwytywanie.PNG");

        }
    }
}
