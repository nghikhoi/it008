using System;
using System.Collections.Concurrent;
using System.IO;
using System.Windows;
using Newtonsoft.Json;

namespace UI.Utils
{
    public class AppConfig
    {
        [JsonIgnore]
        private static String encryptPassword = "Monsuta@!2021#$";

        [JsonProperty("SavedAccount")]
        public ConcurrentDictionary<String, String> SavedAccount = new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public AppConfig()
        {  
        }
        
        public static void Save()
        {
            try
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    String raw = JsonConvert.SerializeObject(Instance);

                    String encrypted = HashUtils.AESEncrypt(raw, encryptPassword);

                    File.WriteAllText(TempUtil.ConfigFilePath, encrypted);
                });
            } catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        
        public static void StartService()
        {
            if (Instance == null)
            {
                if (!File.Exists(TempUtil.ConfigFilePath))
                {
                    Instance = new AppConfig();
                    Save();
                    return;
                }
                
                try
                {
                    String encrypted = File.ReadAllText(TempUtil.ConfigFilePath);
                    String raw = HashUtils.AESDecrypt(encrypted, encryptPassword);
                    Instance = JsonConvert.DeserializeObject<AppConfig>(raw);
                }
                catch {
                    Instance = new AppConfig();
                    Save();
                }
            }
        }

        public static AppConfig Instance { get; private set; }
    }
}
