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
                string json = JsonConvert.SerializeObject(Bot, Formatting.Indented);
                File.WriteAllText(ConfigFolder + "/" + ConfigFile, json);
            }
            else
            {
                string json = File.ReadAllText(ConfigFolder + "/" + ConfigFile);
                Bot = JsonConvert.DeserializeObject<BotConfig>(json);
            }
        }
    }

    public struct BotConfig
    {
        public string Token;
        public string CmdPrefix;
        public string ApiKey;
    }
}