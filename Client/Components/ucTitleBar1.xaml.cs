using MaterialDesignThemes.Wpf;
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
using UI.Command;

namespace UI.Components
{
    /// <summary>
    /// Interaction logic for ucTitleBar1.xaml
    /// </summary>
    public partial class ucTitleBar1 : UserControl
    {
        //public ICommand CloseWindowCommand { get; set; } = new RelayCommand<string>((s => true)
        //   , s => App.Instance.exit());
        //public ICommand MinimizeWindowCommand { get; set; } = new RelayCommand<string>((s => true)
        //    , s => App.Instance.minimized());
        //public ICommand MaximizeWindowCommand { get; set; } = new RelayCommand<string>((s => true)
        //    , s => App.Instance.changeMaximized());

        public ucTitleBar1()
        {
            InitializeComponent();
            DataContext = new TitleBar();
        }

        private void btnFullScreen_Click(object sender, RoutedEventArgs e)
        {
            if (FullScreenIcon.Kind == PackIconKind.Fullscreen)
            {
                FullScreenIcon.Kind = PackIconKind.FullscreenExit;
            }
            else
            {
                FullScreenIcon.Kind = PackIconKind.Fullscreen;
            }
        }
    }
}
