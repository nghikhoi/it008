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
    /// Interaction logic for ucTitleBar.xaml
    /// </summary>
    public partial class ucTitleBar : UserControl {

        public ICommand CloseWindowCommand { get; set; } = new RelayCommand<string>((s => true)
            , s => App.Instance.exit());
        public ICommand MinimizeWindowCommand { get; set; } = new RelayCommand<string>((s => true)
            , s => App.Instance.minimized());

        public ucTitleBar()
        {
            InitializeComponent();
            DataContext = new TitleBar();
        }
    }
}