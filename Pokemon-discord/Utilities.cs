using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Pokemon_discord
{
    class Utilities
    {
        private static readonly Dictionary<string, string> Alerts;

        static Utilities()
        {
            var json = File.ReadAllText("SystemLang/alerts.json");
            Alerts = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }

        //public static string GetAlert(string key)
        //{
        //    if (alerts.ContainsKey(key))
        //    {
        //        return alerts[key];
        //    }
        //    return "No key Found";
        //}

        public static string Get_formatted_alret(string key, params object[] param)
        {
            if (Alerts.ContainsKey(key)) return string.Format(Alerts[key], param);
            return "No key Found";
        }
    }
}