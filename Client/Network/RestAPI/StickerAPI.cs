using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;
using UI.Models.Message;
using UI.Utils.Cache;

namespace UI.Network.RestAPI
{
    public class StickerAPI
    {
        private static readonly String StickerCatesListUrl = "http://{0}:{1}/api/message/sticker/category/list";
        private static readonly String StickerCatesUrl = "http://{0}:{1}/api/message/sticker/category/stickers/{2}";

        private static LFUCache<String, BitmapImage> cachedImage = new LFUCache<string, BitmapImage>(100, 10);

        public delegate void ResultHandler(BitmapImage result);

        public static List<StickerCategory> GetCategories([CallerMemberName] string caller = null)
        {
            using (WebClient client = new WebClient())
            {
                Console.WriteLine(caller + " getting sticker categories...");
                client.Encoding = Encoding.UTF8;
                client.Headers.Add(ClientSession.HeaderToken, ChatConnection.Instance.Session.SessionID);
                String address = ChatConnection.Instance.WebHost;
                int port = ChatConnection.Instance.WebPort;
                String r = client.DownloadString(String.Format(StickerCatesListUrl, address, port));
                return JsonConvert.DeserializeObject<List<StickerCategory>>(r);
            }
        }

        public static List<Sticker> GetStickerCategory(int id, [CallerMemberName] string caller = null) 
        {
            using (WebClient client = new WebClient())
            {
                Console.WriteLine(caller + " getting sticker category...");
                client.Encoding = Encoding.UTF8;
                client.Headers.Add(ClientSession.HeaderToken, ChatConnection.Instance.Session.SessionID);
                String address = ChatConnection.Instance.WebHost;
                int port = ChatConnection.Instance.WebPort;
                String r = client.DownloadString(String.Format(StickerCatesUrl, address, port, id));
                return JsonConvert.DeserializeObject<List<Sticker>>(r);
            }
        }

        public static void DownloadImage(String url, ResultHandler resultHandler, [CallerMemberName] string caller = null)
        {
            String urll = url.ToLower().Trim();
            try
            {
                if (cachedImage.Contains(urll))
                {
                    resultHandler(cachedImage.Get(urll));
                    return;
                }
                new Task(() =>
                {
                    using (WebClient client = new WebClient())
                    {
                        Console.WriteLine(caller + " downloading sticker...");
                        byte[] data = client.DownloadData(url);

                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.StreamSource = new MemoryStream(data);
                        bitmap.EndInit();

                        if (resultHandler != null)
                        {
                            bitmap.Freeze();
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                cachedImage.AddReplace(urll, bitmap);
                                resultHandler(bitmap);
                            });
                        }
                    }
                }).Start();
            } catch (Exception e)
            {
            }
        }

        public static void DownloadImageSynchronously(String url, ResultHandler resultHandler, [CallerMemberName] string caller = null)
        {
            String urll = url.ToLower().Trim();
            try
            {
                if (cachedImage.Contains(urll))
                {
                    resultHandler(cachedImage.Get(urll));
                    return;
                }
                using (WebClient client = new WebClient())
                {
                    Console.WriteLine(caller + " downloading sticker synchronously...");
                    byte[] data = client.DownloadData(url);

                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = new MemoryStream(data);
                    bitmap.EndInit();

                    if (resultHandler != null)
                    {
                        cachedImage.AddReplace(urll, bitmap);
                        resultHandler(bitmap);
                    }
                }
            }
            catch (Exception e)
            {
            }
        }
    }
}
