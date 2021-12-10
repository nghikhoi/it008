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

namespace UI
{
    /// <summary>
    /// Interaction logic for ucInfoConversationSlideBar.xaml
    /// </summary>
    public partial class ConversationInfoSlideBar : UserControl
    {
        public ConversationInfoSlideBar()
        {
            InitializeComponent();
            transitioner.SelectedIndex = 0;
        }

        private void ucMediaGallery_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void BtnMediaGallery_Click_1(object sender, RoutedEventArgs e)
        {
            transitioner.SelectedIndex = 1;
        }

        private void btnBack_Click_1(object sender, RoutedEventArgs e)
        {
            transitioner.SelectedIndex = 0;
        }
    }
}
