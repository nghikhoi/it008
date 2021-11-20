﻿using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using UI.Models;
using UI.Utils;

namespace UI.CustomControls
{
    /// <summary>
    /// Interaction logic for ThumbnailBubble.xaml
    /// </summary>
    public partial class ThumbnailBubble : UserControl
    {
        public ThumbnailBubble()
        {
            InitializeComponent();
        }

        public ThumbnailBubble(MediaInfo mediaInfo) : this()
        {
            if (mediaInfo == null)
                throw new NullReferenceException("MediaInfo cannot be null.");
            this.IsVideoThumbnail = false; //TODO
            this.FileName = mediaInfo.FileName;
            this.FileID = mediaInfo.FileID;
            this.StreamURL = mediaInfo.StreamURL;
            this.ThumbnailUrl = mediaInfo.ThumbURL;
        }

        public ImageSource Image { get => MediaThumb.ImageSource; }

        public String FileName { get; set; }
        public String FileID { get; set; }

        public String StreamURL { get; set; }

        public bool IsVideoThumbnail { get; set; }

        #region Custom Property
        public String ThumbnailUrl
        {
            get { return (String) GetValue(ThumbnailURLProperty); }
            set { SetValue(ThumbnailURLProperty, value); }
        }

        public static readonly DependencyProperty ThumbnailURLProperty = DependencyProperty.Register("ThumbnailUrl", typeof(string), typeof(ThumbnailBubble), new PropertyMetadata(string.Empty));
        #endregion

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e) {
            if (e.Property == ThumbnailURLProperty && !String.IsNullOrEmpty(ThumbnailUrl)) {
                Console.WriteLine("Inside ThumbnailURLProperty");
                Console.WriteLine("Current Threads: {0}", ThreadUtil.GetWorkingThreads());
                LoadingAhihi.Visibility = Visibility.Visible;
                BubbleBkg.Height = BubbleBkg.Width = 96;
                String url = ThumbnailUrl;
                Task task = new Task(() => {
                    try {
                        WebClient wc = new WebClient();
                        //BitmapFrame bitmap = BitmapFrame.Create(new MemoryStream(wc.DownloadData(url)));

                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.StreamSource = new MemoryStream(wc.DownloadData(url));
                        bitmap.EndInit();

                        wc.Dispose();
                        bitmap.Freeze();
                        Application.Current.Dispatcher.Invoke(() => {
                            BubbleBkg.Width = bitmap.PixelWidth;
                            BubbleBkg.Height = bitmap.PixelHeight;

                            if (bitmap.PixelHeight > 250)
                            {
                                BubbleBkg.Height = 250;
                                BubbleBkg.Width = bitmap.PixelWidth * 250 / bitmap.PixelHeight;
                            }
                            else if (bitmap.PixelWidth > 400)
                            {
                                BubbleBkg.Width = 400;
                                BubbleBkg.Height = bitmap.PixelHeight * 400 / bitmap.PixelWidth;
                            }

                            if (IsVideoThumbnail)
                            {
                                PlayIcon.Visibility = Visibility.Visible;
                            }

                            MediaThumb.ImageSource = bitmap;
                            LoadingAhihi.Visibility = Visibility.Hidden;
                        });
                    }
                    catch (WebException we)
                    {
                        Console.WriteLine(we);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                });
                task.Start();
            }

            base.OnPropertyChanged(e);
        }

        public event EventHandler Click;

        private void BtnClick(object sender, RoutedEventArgs e)
        {
            if (Click != null)
                Click(this, e);
        }
        
    }
}
