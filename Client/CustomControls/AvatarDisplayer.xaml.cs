using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UI.Network.RestAPI;

namespace UI.CustomControls
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

        public String UserID
        {
            get { return (String)GetValue(UserIDProperty); }
            set { SetValue(UserIDProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UserID.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UserIDProperty =
            DependencyProperty.Register("UserID", typeof(String), typeof(AvatarDisplayer), new PropertyMetadata(String.Empty));

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == UserIDProperty)
            {
                LoadingMask.Visibility = Visibility.Visible;
                if (String.IsNullOrEmpty(UserID))
                {
                    ProfileAPI.DownloadSelfAvatar((avaSource) =>
                    {
                        this.ImageAva.ImageSource = avaSource;
                        LoadingMask.Visibility = Visibility.Hidden;
                    }, (ex) => Console.WriteLine(ex));
                }
                else
                {
                    ProfileAPI.DownloadUserAvatar(UserID,(avaSource) =>
                    {
                        this.ImageAva.ImageSource = avaSource;
                        LoadingMask.Visibility = Visibility.Hidden;
                    }, (ex) => Console.WriteLine(ex));
                }
            }
            base.OnPropertyChanged(e);
        }

        public void UpdateAllInstance(ImageSource source, String userID = null)
        {
            List<AvatarDisplayer> filtered;
            if (String.IsNullOrEmpty(userID))
            {
                filtered = avatarInstance.Where(p => String.IsNullOrEmpty(p.UserID)).ToList();
            } else
            {
                filtered = avatarInstance.Where(p => p.UserID == userID).ToList();
            }
            foreach (AvatarDisplayer item in filtered)
            {
                item.ImageAva.ImageSource = source;
                item.LoadingMask.Visibility = Visibility.Hidden;
            }
        }

        public void UpdateAllInstance(String userID = null)
        {
            LoadingMask.Visibility = Visibility.Visible;
            if (String.IsNullOrEmpty(userID))
            {
                ProfileAPI.DownloadSelfAvatar((avaSource) =>
                {
                    UpdateAllInstance(avaSource);
                    LoadingMask.Visibility = Visibility.Hidden;
                }, (ex) => Console.WriteLine(ex), true);
            }
            else
            {
                ProfileAPI.DownloadUserAvatar(userID, (avaSource) =>
                {
                    UpdateAllInstance(avaSource, userID);
                    LoadingMask.Visibility = Visibility.Hidden;
                }, (ex) => Console.WriteLine(ex), true);
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
