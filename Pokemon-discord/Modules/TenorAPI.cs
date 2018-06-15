using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon_discord.Modules
{
    internal class TenorAPI
    {
        private static readonly string APIKey = Config.Bot.APIKey;
        private static readonly string _endpoint = "https://api.tenor.com/v1/search?";

        public static string TinyUrl(string query)
        {
            var DataObject = BaseJob(query);
            if (DataObject.next == 0)
            {
                DataObject = BaseJob("Not Found");
            }
            Random r = new Random();
            int rdm = r.Next(Int32.Parse(DataObject.next.ToString()));
            return DataObject.results[rdm].media[0].tinygif.url;
        }

        private static dynamic BaseJob(string query)
        {
            string SearchUrl = $"{_endpoint}+KEY={APIKey}&q={query}";

            var json = "";
            using (var client = new WebClient())
            {
                json = client.DownloadString(SearchUrl);
            }

            return JsonConvert.DeserializeObject<dynamic>(json);
        }
    }
}
