using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Pokemon_discord
{
    internal class Utilities
    {
        private static readonly Dictionary<string, string> alerts;

        static Utilities()
        {
            var json = File.ReadAllText("SystemLang/alerts.json");
            alerts = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
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
            if (alerts.ContainsKey(key)) return string.Format(alerts[key], param);
            return "No key Found";
        }
    }
}