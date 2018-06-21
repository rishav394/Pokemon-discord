using System;
using System.Net;
using Newtonsoft.Json;

namespace Pokemon_discord.ModuleHelper
{
    internal class TenorApi
    {
        private static readonly string ApiKey = Config.Bot.TenorApiKey;
        private static readonly string Endpoint = "https://api.tenor.com/v1/search?";

        public static string TinyUrl(string query)
        {
            dynamic dataObject = BaseJob(query);
            if (dataObject.next < 1) dataObject = BaseJob("Not Found");
            var r = new Random();
            int rdm = r.Next(dataObject.results.Count);
            return dataObject.results[rdm].media[0].tinygif.url;
        }

        internal static string BigUrl(string query)
        {
            dynamic dataObject = BaseJob(query);
            if (dataObject.next < 1) dataObject = BaseJob("Not Found");
            var r = new Random();
            int rdm = r.Next(dataObject.results.Count);
            return dataObject.results[rdm].media[0].gif.url;
        }

        private static dynamic BaseJob(string query)
        {
            string searchUrl = $"{Endpoint}KEY={ApiKey}&q={query}";
            string json;
            using (var client = new WebClient())
            {
                json = client.DownloadString(searchUrl);
            }

            return JsonConvert.DeserializeObject<dynamic>(json);
        }
    }
}