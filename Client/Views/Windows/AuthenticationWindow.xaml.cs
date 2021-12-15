using System.Windows;

namespace UI.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class AuthenticationWindow : Window
    {
        public AuthenticationWindow(object dataContext)
        {
            InitializeComponent();
            this.DataContext = dataContext;
        }
    }
        
}
