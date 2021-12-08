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

namespace UI.Components
{
    /// <summary>
    /// Interaction logic for ucMediaGallery.xaml
    /// </summary>
    public partial class MediaGalleryTabs : UserControl
    {
        private Border[] tabMarks = new Border[2];

        public MediaGalleryTabs()
        {
            InitializeComponent();
            transitioner.SelectedIndex = 0;
            tabMarks[0] = MeidiaTabSelectMark;
            tabMarks[1] = FilesRabSelectMark;
            selectTab(0);
        }

        private void ucTitleBar_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void selectTab(int index)
        {
            foreach (var mark in tabMarks) mark.Visibility = Visibility.Hidden;
            tabMarks[index].Visibility = Visibility.Visible;
        }

        private void btnMediaTab_Click(object sender, RoutedEventArgs e)
        {
            transitioner.SelectedIndex = 0;
            selectTab(0);
        }

        private void btnFilesTab_Click(object sender, RoutedEventArgs e)
        {
            transitioner.SelectedIndex = 1;
            selectTab(1);
        }
    }
}
