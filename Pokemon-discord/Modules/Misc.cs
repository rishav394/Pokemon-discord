using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using NReco.ImageGenerator;
using Pokemon_discord.Core.UserAccounts;
using Newtonsoft.Json;

namespace Pokemon_discord.Modules
{
    public class Misc : ModuleBase<SocketCommandContext>  
    {
        private const string ThumbnailUrl = "https://assets.pokemon.com/static2/_ui/img/global/three-characters.png";
        private const string AbandonURL = "https://cdn.shopify.com/s/files/1/1024/7339/files/logoB_large.png?14615439934852209744";
        private readonly string TargetRole = "Moderator";

        [Command("try")]
        [Summary("Found out that we cant actuall store socketUser in json. Serialisation error")]
        public async Task Tryg(SocketUser socketUser)
        {
            //
            string json = JsonConvert.SerializeObject(socketUser, Formatting.Indented);
            File.WriteAllText("bird.json", json);
            //
        }

        [Command("rep")]
        public async Task Repped(SocketUser socketUser)
        {
            var account = UserAccounts.GetAccount(Context.User);

            if (account.RepperList.Contains(socketUser.Id))
            {
                await ReplyAsync("Hey Hey Hey you already did it once.");
                var AllTheRepped = from a in Context.Guild.Users
                                   where account.RepperList.Contains(a.Id)
                                   select a;
                var embed = new EmbedBuilder();
                embed.WithTitle("You have repped all these lads so far");
                int temp = 1;
                foreach (var a in AllTheRepped)
                {
                    embed.AddField(temp++.ToString(), a.Username);
                }
                await ReplyAsync("", false, embed.Build());
            }
            else
            {
                account.RepperList.Add(socketUser.Id);
                account.Countem++;
                await ReplyAsync("Okay you +repped " + socketUser.Username);
            }
            UserAccounts.SaveAccounts();
        }

        [Command("yify")]
        public async Task GetYify([Remainder]string args = "")
        {
            string json = "";
            using (WebClient client = new WebClient())
            {
                json = client.DownloadString("https://yts.am/api/v2/list_movies.json?quality=3D");
            }

            var dataObject = JsonConvert.DeserializeObject<dynamic>(json);
            string movie_count = dataObject.data.movie_count.ToString();
            string status = dataObject.status_message.ToString();
            string first_title = dataObject.data.movies[0].title.ToString();

            var embed = new EmbedBuilder();
            embed.WithTitle("YIFY returned the following response");
            embed.WithColor(Color.Blue);
            embed.AddField("Status", status);
            embed.AddField("Total results", movie_count);
            embed.AddField("Movie Name", first_title);
            embed.WithThumbnailUrl("https://yts.am/assets/images/website/og_yts_logo.png");

            await Context.Channel.SendMessageAsync("", false, embed.Build());

            //await ReplyAsync($"Yify returned {status} with movies count of {movie_count} and the firt movie is {first_title}");
        }
        
        [Command("Person")]
        public async Task GetRandom([Remainder]string args = "")
        {
            string json = "";
            using (WebClient client = new WebClient())
            {
                json = client.DownloadString("https://randomuser.me/api/?gender=male&nat=ru");
            }

            var dataObject = JsonConvert.DeserializeObject<dynamic>(json);
            string gender = dataObject.results[0].gender.ToString();
            string middlename = dataObject.results[0].name.first.ToString();
            string pic = dataObject.results[0].picture.large.ToString();
            await ReplyAsync($"Gender : {middlename} and {pic}");
        }
        
        [Command("hello")]
        public async Task ImageShit([Remainder]string args = "")
        {
            string css = "<style>\n	h1{\n		color: rgb(27,82,122);\n	}\n";
            string html = String.Format("<h1>Hello {0}</h1>", Context.User.Username);
            var jpegBytes = new HtmlToImageConverter
            {
                Width = 200,
                Height = 70
            }.GenerateImage(css + html, NReco.ImageGenerator.ImageFormat.Jpeg);
            await Context.Channel.SendFileAsync(new MemoryStream(jpegBytes), "Hello.jpeg");
        }
        
        [Command("Abandon ship")]
        public async Task ShutDown()
        {
            var embed = new EmbedBuilder()
                .WithColor(Color.Blue)
                .WithTitle("Wait What????")
                .WithDescription(Utilities.Get_formatted_alret("ABANDON"))
                .WithThumbnailUrl(AbandonURL)
                .Build();

            await Context.Channel.SendMessageAsync("", false, embed);

            var dmChannel = await Context.User.GetOrCreateDMChannelAsync();
            await dmChannel.SendMessageAsync("Shutdown complete.");
            await dmChannel.SendMessageAsync("`Environment.Exit(1)`");


            Environment.Exit(1);
        }
        
        [Command("Stats")]
        public async Task StatsOther(SocketUser socketUser = null)
        {
            if (socketUser == null)
            {
                socketUser = Context.User;
            }
            var account = UserAccounts.GetAccount(socketUser);
            await Context.Channel.SendMessageAsync($"Hey {socketUser.Mention}, You have {account.Size} long sandwhiches and {account.XP} XP.");
        }
        
        [Command("Daily")]
        public async Task Daily(SocketUser socketUser = null)
        {
            if (socketUser == null)
            {
                socketUser = Context.User;
                var account = UserAccounts.GetAccount(socketUser);
                account.XP += 200;
                await Context.Channel.SendMessageAsync($"Hey {socketUser.Mention}, You gained 200 XP.");
                UserAccounts.SaveAccounts();
            }
            else
            {
                if (IsUserRoleHolder((SocketGuildUser)Context.User, TargetRole))
                {
                    var account = UserAccounts.GetAccount(socketUser);
                    account.XP += 200;
                    await Context.Channel.SendMessageAsync($"Hey {socketUser.Mention}, You gained 200 XP.");
                    UserAccounts.SaveAccounts();
                }
                else
                {
                    await Context.Channel.SendMessageAsync($"Mmm sorry {Context.User.Mention}, You need to be a {TargetRole} to be able to do so");
                }
            }
        }
        
        [Command("Pick")]
        public async Task PickOne([Remainder]string message)
        {
            string[] options = message.Split(new char[] { '|' , ' ' }, StringSplitOptions.RemoveEmptyEntries);
            Random r = new Random();


            var embed = new EmbedBuilder()
                .WithTitle("Requested by " + Context.User)
                .WithDescription(options[r.Next(0, options.Length)])
                .WithColor(Color.Blue)
                .WithThumbnailUrl(ThumbnailUrl)
                .Build();

            await Context.Channel.SendMessageAsync("", false, embed);
        }
        
        [Command("secret")]
        public async Task RevealSecret([Remainder]string arguments = "") 
        {
            ///<summary>
            ///User is not secret owner. Denied.
            /// </summary>
            if (!IsUserRoleHolder((SocketGuildUser)(Context.User), TargetRole)) 
            {
                Console.WriteLine("User is not secret owner");
                await Context.Channel.SendMessageAsync(Utilities.Get_formatted_alret("PERMISSION_DENIED", Context.User.Mention));
                return;
            }

            ///<summary>
            ///User is secret owner. Sending DM
            /// </summary>
            var dmChannel = await Context.User.GetOrCreateDMChannelAsync();
            await Context.Channel.SendMessageAsync(Utilities.Get_formatted_alret("CHECK_DM", Context.User.Mention));        //Send check_dm in the channel


            var embed = new EmbedBuilder()
                .WithTitle("Requested by " + Context.User)
                .WithDescription(Utilities.Get_formatted_alret("SECRET"))
                .WithColor(Color.Blue)
                .WithThumbnailUrl(ThumbnailUrl)
                .Build();


            await dmChannel.SendMessageAsync("", false, embed);                     // Send embeed in DM
        }
        
        [Command("Add")]
        public async Task GetData([Remainder]string arguments)
        {
            DataStorage.pairs.Add(arguments.Split('=', ';', '|')[0], arguments.Split('=', ';', '|')[1]);
            DataStorage.SaveData();
            await Context.Channel.SendMessageAsync("DataStorage has " + DataStorage.pairs.Count + " pairs.");
        }
        
        [Command("Find")]
        public async Task FindData([Remainder]string key)
        {
            await Context.Channel.SendMessageAsync(DataStorage.pairs[key]);
        }
        
        [Command("Delete")]
        public async Task DeleteData([Remainder]string key = null)
        {
            if (key == null)
            {
                DataStorage.pairs.Clear();
            }
            else
            {
                Console.WriteLine($"Trying to remove {key}");
                DataStorage.pairs.Remove(key);
            }
            DataStorage.SaveData();
            await Context.Channel.SendMessageAsync("DataStorage has " + DataStorage.pairs.Count + " pairs.");
        }

        [Command("View")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task PrintData()
        {
            string desc = null;
            foreach ( KeyValuePair<string,string> x in DataStorage.pairs)
            {
                desc += x.Key + " " + x.Value + "\n";
            }
            desc += "\b";
            var embed = new EmbedBuilder()
            .WithTitle("Requested by " + Context.User.Mention)
            .WithColor(Color.Gold)
            .WithDescription(desc)
            .WithThumbnailUrl(ThumbnailUrl)
            .WithImageUrl("")
            .Build();

            await Context.Channel.SendMessageAsync("", false, embed);
        }


        public bool IsUserRoleHolder(SocketGuildUser user, string targetRoleName) 
        {
            var result = from r in user.Guild.Roles
                         where r.Name == targetRoleName
                         select r.Id;
            ulong targetRoleID = result.FirstOrDefault();
            if (targetRoleID == 0)
            {
                Console.WriteLine("Target role was not found :( ");
                return false;
            }

            var targetRole = user.Guild.GetRole(targetRoleID);
            return user.Roles.Contains(targetRole);
        }
    }
}
