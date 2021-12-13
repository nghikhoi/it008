using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using UI.Models;
using UI.ViewModels;

namespace UI.Utils.Converters {
    public class ThumbnailLoader : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            string url;
            if (value is string s) {
                url = s;
            } else if (value is MediaInfo info) {
                url = info.ThumbURL;
            } else if (value is MediaViewModel model) {
                url = model.MediaInfo.ThumbURL;
            } else
                return Binding.DoNothing;
            ThumbnailProvider provider = new ThumbnailProvider();
            Task task = new Task(() =>
            {
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
                        provider.Thumbnail = bitmap;
                    });
                } catch (WebException we) {
                    Console.WriteLine(we);
                } catch (Exception ex) {
                    Console.WriteLine(ex);
                }
            });
            task.Start();
            return provider;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }

    public class ThumbnailProvider : ViewModelBase {

        private BitmapImage _thumbnail;
        public BitmapImage Thumbnail {
            get => _thumbnail; 
            set {
                _thumbnail = value;
                OnPropertyChanged(nameof(Thumbnail));
            }
        }

    }

}
