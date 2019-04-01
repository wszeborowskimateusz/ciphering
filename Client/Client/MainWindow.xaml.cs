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

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            TCPServer tCPServer = new TCPServer();
            Task.Run(() => tCPServer.StartListening());

            FileDecoder fileDecoder = new FileDecoder();
            //fileDecoder.GetFileHeader("C:\\Users\\Mateusz\\Desktop\\BSK_projekty\\Ciphering\\OutputFiles\\Przechwytywanie.PNG");
            Console.WriteLine(fileDecoder.DecryptPrivateKey("user5", "alamakota"));
        }
    }
}
