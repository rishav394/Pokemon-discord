using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace Pokemon_discord
{
    class Utilities
    {
        private static Dictionary<string, string> alerts;

        static Utilities()
        {
            string json = File.ReadAllText("SystemLang/alerts.json");
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

        public static string Get_formatted_alret(string key,params object[] param) 
        {
            if (alerts.ContainsKey(key))
            {
                return String.Format(alerts[key], param);
            }
            return "No key Found";
        }

    }
}
