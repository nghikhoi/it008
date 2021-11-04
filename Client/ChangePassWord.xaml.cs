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
using System.Windows.Shapes;

namespace UI
{
    /// <summary>
    /// Interaction logic for ChangePassWord.xaml
    /// </summary>
    public partial class ChangePassWord : Window
    {
        public ChangePassWord()
        {
            InitializeComponent();
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CloseOnDeactivated(object sender, EventArgs e)
        {
            Window win = sender as Window;
            win.Close();
        }
    }
}
