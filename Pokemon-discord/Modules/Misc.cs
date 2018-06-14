using System;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;


namespace Pokemon_discord.Modules
{
    public class Misc : ModuleBase<SocketCommandContext>
    {
        private const string AbandonURL = "https://cdn.shopify.com/s/files/1/1024/7339/files/logoB_large.png?14615439934852209744";
        private const string ThumbnailUrl = "https://assets.pokemon.com/static2/_ui/img/global/three-characters.png";
        

        [Command("Try")]
        [Summary("Found out that we cant actuall store socketUser in json. Serialisation error")]
        public async Task Tryg(SocketUser socketUser)
        {
            //
            string json = JsonConvert.SerializeObject(socketUser, Formatting.Indented);
            File.WriteAllText("bird.json", json);
            //
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
            string targetRoleName = "Moderator";
            ///<summary>
            ///User is not secret owner. Denied.
            /// </summary>
            if (!IsUserRoleHolder((SocketGuildUser)(Context.User), targetRoleName)) 
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
        


        public static bool IsUserRoleHolder(SocketGuildUser user, string targetRoleName) 
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
