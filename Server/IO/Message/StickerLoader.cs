using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatServer.Entity;

namespace ChatServer.IO.Message
{
    public class StickerLoader : Store
    {
        static readonly string CategoryPath = "Stickers/Categories.json";
        static readonly string StickerPath = "Stickers/Stickers.json";

        public List<StickerCategory> LoadCategory()
        {
            List<StickerCategory> result = new List<StickerCategory>();

            try
            {
                StreamReader reader = new StreamReader(CategoryPath);
                string json = reader.ReadToEnd();

                result = JsonConvert.DeserializeObject<List<StickerCategory>>(json);
            } catch (Exception e)
            {
                SimpleChatServer.GetServer().Logger.Error(e);
            }

            return result;
        }

        public List<List<Sticker>> LoadSticker()
        {
            List<List<Sticker>> result = new List<List<Sticker>>();

            try
            {
                StreamReader reader = new StreamReader(StickerPath);
                string json = reader.ReadToEnd();

                result = JsonConvert.DeserializeObject<List<List<Sticker>>>(json);
            } catch (Exception e)
            {
                SimpleChatServer.GetServer().Logger.Error(e);
            }

            return result;
        }
    }
}
