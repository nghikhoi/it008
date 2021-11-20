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
    /// Interaction logic for ChangeName.xaml
    /// </summary>
    public partial class ChangeName : Window
    {
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        public ChangeName(string firstName = "", string lastName = "") {
            FirstName = firstName;
            LastName = lastName;
            InitializeComponent();
        }
        
        private void BackButton_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void acceptButtonClick(object sender, RoutedEventArgs e) {
            this.DialogResult = true;
        }
        
    }
}
