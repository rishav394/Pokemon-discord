using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;


namespace Pokemon_discord
{
    internal class Config
    {
        private const string ConfigFolder = "Resources";
        private const string ConfigFile = "config.json";

        public static BotConfig Bot;

        static Config()
        {
            if (!Directory.Exists(ConfigFolder))
                Directory.CreateDirectory(ConfigFolder);

            if (!File.Exists(ConfigFolder + "/" + ConfigFile))
            {
                Bot = new BotConfig();
                Bot.PrefixDictionary=new Dictionary<ulong, string>();
                string json = JsonConvert.SerializeObject(Bot, Formatting.Indented);
                File.WriteAllText(ConfigFolder + "/" + ConfigFile, json);
            }
            else
            {
                string json = File.ReadAllText(ConfigFolder + "/" + ConfigFile);
                Bot = JsonConvert.DeserializeObject<BotConfig>(json);
            }
        }

        public static void SavePrefix()
        {
            string json = JsonConvert.SerializeObject(Bot, Formatting.Indented);
            File.WriteAllText(ConfigFolder + "/" + ConfigFile, json);
        }
    }

    public struct BotConfig
    {
        public string Token;
        public Dictionary<ulong, string> PrefixDictionary;
        public string TenorApiKey;
        public string TranslateApiKey;
    }
}