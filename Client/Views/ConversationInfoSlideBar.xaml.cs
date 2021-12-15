using System.Windows;
using System.Windows.Controls;

namespace UI.Views
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
