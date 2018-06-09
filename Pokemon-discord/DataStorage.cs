using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace Pokemon_discord
{
    class DataStorage
    {
        public static Dictionary<string, string> pairs = new Dictionary<string, string>();
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
            string json = File.ReadAllText(FileLocation);
            pairs = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }

        public static void SaveData()
        {
            string json = JsonConvert.SerializeObject(pairs, Formatting.Indented);
            File.WriteAllText(FileLocation, json);
        }

    }

}
