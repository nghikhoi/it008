using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UI.Network.RestAPI;

namespace UI.Components
{
    /// <summary>
    /// Interaction logic for AvatarDisplayer.xaml
    /// </summary>
    public partial class AvatarDisplayer : UserControl
    {
        private static List<AvatarDisplayer> avatarInstance = new List<AvatarDisplayer>();

        public AvatarDisplayer()
        {
            InitializeComponent();
        }

        public string UserID {
            get { return (string) GetValue(UserIDProperty); }
            set { SetValue(UserIDProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UserID.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UserIDProperty =
            DependencyProperty.Register(nameof(UserID), typeof(string), typeof(AvatarDisplayer), new FrameworkPropertyMetadata(null));

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == UserIDProperty)
            {
                Update();
            }
            base.OnPropertyChanged(e);
        }

        private void Update() {
            LoadingMask.Visibility = Visibility.Visible;
            if (String.IsNullOrEmpty(UserID)) {
                ProfileAPI.DownloadSelfAvatar((avaSource) =>
                {
                    this.ImageAva.ImageSource = avaSource;
                    LoadingMask.Visibility = Visibility.Hidden;
                }, (ex) => Console.WriteLine(ex));
            } else {
                ProfileAPI.DownloadUserAvatar(UserID, (avaSource) =>
                {
                    this.ImageAva.ImageSource = avaSource;
                    LoadingMask.Visibility = Visibility.Hidden;
                }, (ex) => Console.WriteLine(ex));
            }
        }

        public static void UpdateAllInstance(String userID = null)
        {
            foreach (var avatarDisplayer in avatarInstance.Where(target => string.CompareOrdinal(target.UserID, userID) == 0))
            {
                avatarDisplayer.Update();
            }
        }

        private void OnUnload(object sender, RoutedEventArgs e)
        {
            avatarInstance.Remove(this);
        }

        #region Custom ClickEvent
        public event EventHandler Click;

        private void BtnClick(object sender, RoutedEventArgs e)
        {
            if (Click != null)
                Click(this, e);
        }
        #endregion

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            AvatarDisplayer demo = avatarInstance.Where(p => p.UserID == this.UserID).FirstOrDefault();
            if (demo != null)
            {
                this.ImageAva.ImageSource = demo.ImageAva.ImageSource;
                this.LoadingMask.Visibility = Visibility.Hidden;
            } else
            {
                this.UserID = this.UserID;
            }
            if (!avatarInstance.Contains(this))
                avatarInstance.Add(this);
        }
    }
}
