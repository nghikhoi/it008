using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using UI.Network.RestAPI;
using WpfAnimatedGif;

namespace UI.CustomControls
{
    /// <summary>
    /// Interaction logic for Sticker.xaml
    /// </summary>
    public partial class Sticker : UserControl
    {

        public Sticker(bool clickable, int id, int cateid, int size, int duration, string urisheet, bool runWhenLoaded)
        {
            InitializeComponent();
            
            ID = id;
            CateID = cateid;
            Size = size;
            Duration = duration;
            UriSheet = urisheet;
            Clickable = clickable;
            IsRunWhenLoaded = runWhenLoaded;
        }

        public bool IsRunWhenLoaded { get; set; }

        public bool Clickable { get; set; }


        public int ID
        {
            get; set;
        }

        public int CateID
        {
            get; set;
        }

        public int Size
        {
            get; set;
        }

        public int Duration
        {
            get; set;
        }

        public string UriSheet
        {
            get; set;
        }

        private void loadSticker() {
            Image gif = new Image();
            RepeatBehavior behavior = new RepeatBehavior();
            ImageBehavior.SetRepeatBehavior(gif, new RepeatBehavior(5));
            ImageBehavior.SetAnimationDuration(gif, new Duration(new TimeSpan(Duration / 1000)));
            if (UriSheet != null) {
                progressBar.Visibility = Visibility.Visible;
                StickerAPI.DownloadImage(UriSheet, (stickerSheet) =>
                {
                    ImageBehavior.SetAnimatedSource(gif, stickerSheet);
                    gif.Width = Size;
                    gif.Height = Size;
                    sticker.Children.Add(gif);
                    progressBar.Visibility = Visibility.Hidden;
                });
            }
            
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            loadSticker();
        }
    }
}
