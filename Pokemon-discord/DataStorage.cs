using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Pokemon_discord
{
    internal class DataStorage
    {
        public static Dictionary<string, string> Pairs = new Dictionary<string, string>();
        private static readonly string FileLocation = "DataStorage.json";

        static DataStorage()
        {
            //Load data
            if (!File.Exists(FileLocation))
            {
                File.WriteAllText(FileLocation, "");
                SaveData();
                return;
            }

            var json = File.ReadAllText(FileLocation);
            Pairs = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }

        public static void SaveData()
        {
            var json = JsonConvert.SerializeObject(Pairs, Formatting.Indented);
            File.WriteAllText(FileLocation, json);
        }
    }
}