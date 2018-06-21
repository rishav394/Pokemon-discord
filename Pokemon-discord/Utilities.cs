using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Pokemon_discord
{
    internal static class Utilities
    {
        private static readonly Dictionary<string, string> Alerts;

        static Utilities()
        {
            string json = File.ReadAllText("SystemLang/alerts.json");
            Alerts = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }

        public static string Get_formatted_alret(string key, params object[] param)
        {
            return Alerts.ContainsKey(key) ? string.Format(Alerts[key], param) : "No key Found";
        }
    }
}