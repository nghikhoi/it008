using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
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
using UI.Models;
using UI.Utils;

namespace UI.Components {
    /// <summary>
    /// Interaction logic for ImageLoader.xaml
    /// </summary>
    public partial class ImageLoader : UserControl {
		public static readonly DependencyProperty LoadingProgressProperty = DependencyProperty.Register(
			nameof(LoadingProgress), typeof(double)
			, typeof(ImageLoader), new FrameworkPropertyMetadata(0d));

		public double LoadingProgress {
			get => (double) GetValue(LoadingProgressProperty);
			set => SetValue(LoadingProgressProperty, value);
		}

		public static readonly DependencyProperty MediaLoadedProperty = DependencyProperty.Register(nameof(MediaLoaded),
			typeof(bool)
			, typeof(ImageLoader), new FrameworkPropertyMetadata(false));

		public bool MediaLoaded {
			get => (bool) GetValue(MediaLoadedProperty);
			set => SetValue(MediaLoadedProperty, value);
		}

		public static readonly DependencyProperty MediaSourceProperty = DependencyProperty.Register(nameof(MediaSource),
			typeof(object)
			, typeof(ImageLoader), new FrameworkPropertyMetadata(null));

		public object MediaSource {
			get => GetValue(MediaSourceProperty);
			set => SetValue(MediaSourceProperty, value);
		}

		public static readonly DependencyProperty MediaInfoProperty = DependencyProperty.Register(nameof(MediaInfo),
			typeof(string)
			, typeof(ImageLoader), new FrameworkPropertyMetadata(null));

		public string MediaInfo {
			get => GetValue(MediaInfoProperty) as string;
			set => SetValue(MediaInfoProperty, value);
		}

		public ImageLoader() {
			InitializeComponent();
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e) {
			if (e.Property == MediaInfoProperty) {
				UpdateSource();
			}

			base.OnPropertyChanged(e);
		}

		private Thread _imageLoadingThread;

		private void UpdateSource() {
			MediaLoaded = false;
			if (!FastCodeUtils.NotEmptyStrings(MediaInfo))
				return;
            string cloneStr = MediaInfo;
            Task task = new Task(() => {
                try {
                    WebClient wc = new WebClient();
                    //BitmapFrame bitmap = BitmapFrame.Create(new MemoryStream(wc.DownloadData(url)));

                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = new MemoryStream(wc.DownloadData(cloneStr));
                    bitmap.EndInit();

                    wc.Dispose();
                    bitmap.Freeze();
                    Application.Current.Dispatcher.Invoke(() => {
                        MediaSource = bitmap;
                        MediaLoaded = true;
                    });
                } catch (WebException we) {
                    Console.WriteLine(we);
                } catch (Exception ex) {
                    Console.WriteLine(ex);
                }
            });
            task.Start();
        }

	}

}
