using Newtonsoft.Json;
using System;
using System.Net;

namespace Pokemon_discord.Modules
{
    internal class TenorAPI
    {
        private static readonly string APIKey = Config.Bot.APIKey;
        private static readonly string _endpoint = "https://api.tenor.com/v1/search?";

        public static string TinyUrl(string query)
        {
            var DataObject = BaseJob(query);
            if (DataObject.next < 1)
            {
                DataObject = BaseJob("Not Found");
            }
            Random r = new Random();
            int rdm = r.Next(DataObject.results.Count);
            return DataObject.results[rdm].media[0].tinygif.url;
        }


        internal static string BigUrl(string query)
        {
            var DataObject = BaseJob(query);
            if (DataObject.next < 1)
            {
                DataObject = BaseJob("Not Found");
            }
            Random r = new Random();
            int rdm = r.Next(DataObject.results.Count);
            return DataObject.results[rdm].media[0].gif.url;
        }

        private static dynamic BaseJob(string query)
        {
            string SearchUrl = $"{_endpoint}KEY={APIKey}&q={query}";

            var json = "";
            using (var client = new WebClient())
            {
                json = client.DownloadString(SearchUrl);
            }

            return JsonConvert.DeserializeObject<dynamic>(json);
        }

    }
}
