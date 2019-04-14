using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UCFilePicker
{
    public class ViewDecoder
    {
        public ViewDecoder(FilePicker uc)
        {
            _UCFilePicker = uc;
            _UCFilePicker.Button_BrowseFiles.Click += Button_BrowseFiles_Click;
        }

        private void Button_BrowseFiles_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Implement me!");
        }

        private FilePicker _UCFilePicker;
    }
}
