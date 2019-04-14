using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UCCipher
{
    public class ViewDecoder
    {
        public ViewDecoder(Cipher uc)
        {
            _UCCipher = uc;
            _UCCipher.Button_Initialize_Cipher.Click += Button_Initialize_Cipher_Click;
            _UCCipher.Button_Send.Click += Button_Send_Click;
        }

        private void Button_Send_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Implement me!");
        }

        private void Button_Initialize_Cipher_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Implement me!");
        }

        private Cipher _UCCipher;
    }
}
