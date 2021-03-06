﻿using System.Net;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Newtonsoft.Json;

namespace Pokemon_discord.Modules
{
    public class ApIs : ModuleBase<SocketCommandContext>
    {
        [Command("Yify")]
        public async Task GetYify([Remainder] string args = "")
        {
            string json;
            using (var client = new WebClient())
            {
                json = client.DownloadString("https://yts.am/api/v2/list_movies.json?quality=3D");
            }

            var dataObject = JsonConvert.DeserializeObject<dynamic>(json);
            string movieCount = dataObject.data.movie_count.ToString();
            string status = dataObject.status_message.ToString();
            string firstTitle = dataObject.data.movies[0].title.ToString();
            var embed = new EmbedBuilder();
            embed.WithTitle("YIFY returned the following response");
            embed.WithColor(Color.Blue);
            embed.AddField("Status", status);
            embed.AddField("Total results", movieCount);
            embed.AddField("Movie Name", firstTitle);
            embed.WithThumbnailUrl("https://yts.am/assets/images/website/og_yts_logo.png");
            await Context.Channel.SendMessageAsync("", false, embed.Build());

            //await ReplyAsync($"Yify returned {status} with movies count of {movie_count} and the firt movie is {first_title}");
        }

        [Command("Person")]
        public async Task GetRandom([Remainder] string args = "")
        {
            string json;
            using (var client = new WebClient())
            {
                json = client.DownloadString("https://randomuser.me/api/?gender=male&nat=ru");
            }

            var dataObject = JsonConvert.DeserializeObject<dynamic>(json);
            string gender = dataObject.results[0].gender.ToString();
            string middlename = dataObject.results[0].name.first.ToString();
            string pic = dataObject.results[0].picture.large.ToString();
            await ReplyAsync($"Gender :{gender} & {middlename} and {pic}");
        }
    }
}