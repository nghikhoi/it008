using System;
using System.Windows.Controls;

namespace UI.Components
{
    /// <summary>
    /// Interaction logic for AnnouncementDialog.xaml
    /// </summary>
    public partial class AnnouncementDialog : UserControl
    {
        public AnnouncementDialog(params string[] messages)
        {
            InitializeComponent();
            announceText.Text = String.Join("\n", messages);
        }
    }
}
