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
    /// Interaction logic for ChatListControl.xaml
    /// </summary>
    public partial class ChatListControl : UserControl
    {
        public ChatListControl()
        {
            InitializeComponent();
        }
        public void AddListItem()
        {
            ChatListItemControl chatListItemControl = new ChatListItemControl();
            this.Container.Children.Add(chatListItemControl);
        }

        private void AddListTest(object sender, KeyEventArgs e)
        {
            AddListItem();
        }
    }
}
