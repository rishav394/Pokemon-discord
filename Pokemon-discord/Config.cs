using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;


namespace Pokemon_discord
{
    class Config
    {
        private const string configfile = "config.json";
        private const string configfolder = "Resources";

        public static BotConfig bot;

        static Config()
        {
            if (!Directory.Exists(configfolder))
            {
                Directory.CreateDirectory(configfolder);
            }
            if (!File.Exists(configfolder + "/" + configfile))
            {
                bot = new BotConfig();
                string json = JsonConvert.SerializeObject(bot, Formatting.Indented);
                File.WriteAllText(configfolder + "/" + configfile, json);
            }
            else
            {
                string json = File.ReadAllText(configfolder + "/" + configfile);
                bot = JsonConvert.DeserializeObject<BotConfig>(json);
            }
        }
    }

    public struct BotConfig
    {
        public string token;
        public string cmdPrefix;
    }
}
