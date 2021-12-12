using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using UI.Models;

namespace UI.Components {
	public partial class MediaLoader : UserControl {
		public static readonly DependencyProperty LoadingProgressProperty = DependencyProperty.Register(
			nameof(LoadingProgress), typeof(double)
			, typeof(MediaLoader), new FrameworkPropertyMetadata(0d));

		public double LoadingProgress {
			get => (double) GetValue(LoadingProgressProperty);
			set => SetValue(LoadingProgressProperty, value);
		}

		public static readonly DependencyProperty MediaLoadedProperty = DependencyProperty.Register(nameof(MediaLoaded),
			typeof(bool)
			, typeof(MediaLoader), new FrameworkPropertyMetadata(false));

		public bool MediaLoaded {
			get => (bool) GetValue(MediaLoadedProperty);
			set => SetValue(MediaLoadedProperty, value);
		}

		public static readonly DependencyProperty MediaSourceProperty = DependencyProperty.Register(nameof(MediaSource),
			typeof(object)
			, typeof(MediaLoader), new FrameworkPropertyMetadata(null));

		public object MediaSource {
			get => GetValue(MediaSourceProperty);
			set => SetValue(MediaSourceProperty, value);
		}

		public static readonly DependencyProperty MediaInfoProperty = DependencyProperty.Register(nameof(MediaInfo),
			typeof(MediaInfo)
			, typeof(MediaLoader), new FrameworkPropertyMetadata(null));

		public MediaInfo MediaInfo {
			get => GetValue(MediaInfoProperty) as MediaInfo;
			set => SetValue(MediaInfoProperty, value);
		}

		public MediaLoader() {
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
			if (MediaInfo.IsVideo()) {
				MediaSource = new Uri(MediaInfo.StreamURL);
				MediaLoaded = true;
				LoadingProgress = 100;
			}
			else {
				if (_imageLoadingThread != null && _imageLoadingThread.IsAlive)
					_imageLoadingThread.Abort();
				_imageLoadingThread = new Thread(() => {
					try {
						WebClient wc = new WebClient();
						wc.DownloadProgressChanged += (sender, args) => {
							Application.Current.Dispatcher.Invoke(() => {
								LoadingProgress = (double) args.BytesReceived / args.TotalBytesToReceive;
							});
						};
						BitmapFrame bitmap = BitmapFrame.Create(new MemoryStream(wc.DownloadData(MediaInfo.StreamURL)));
						wc.Dispose();
						Application.Current.Dispatcher.Invoke(() => {
							// ImgCache.AddReplace(imageURL, bitmap); //TODO Cache
							MediaSource = bitmap;
							/*ImgFull.MaxHeight = bitmap.PixelHeight;
							ImgFull.MaxWidth = bitmap.PixelWidth;*/
							MediaLoaded = true;
						});
					}
					catch (Exception ex) {
						Console.WriteLine(ex);
					}
				});
				_imageLoadingThread.IsBackground = true;
				_imageLoadingThread.Start();
			}
		}
	}
	
	public class MediaDisplayerSelector : DataTemplateSelector {
		
		public override DataTemplate SelectTemplate(object item, DependencyObject container) {
			FrameworkElement element = container as FrameworkElement;
			if (element != null && item != null && item is MediaInfo) {
				MediaInfo info = item as MediaInfo;

				if (info.IsVideo()) {
					return element.FindResource("VideoMedia") as DataTemplate;
				}
				else {
					return element.FindResource("ImageMedia") as DataTemplate;
				}
			}
			return null;
		}
		
	}
}