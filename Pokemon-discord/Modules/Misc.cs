using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Pokemon_discord.Core.UserAccounts;

namespace Pokemon_discord.Modules
{
    public class Misc : ModuleBase<SocketCommandContext>  
    {
        private const string ThumbnailUrl = "https://assets.pokemon.com/static2/_ui/img/global/three-characters.png";
        

        [Command("Stats")]
        public async Task Stats()
        {
            var account = UserAccounts.GetAccount(Context.User);
            await Context.Channel.SendMessageAsync($"Hey {Context.User.Mention}, You have {account.Size} long sandwhiches and {account.XP} XP.");
        }

        [Command("Stats")]
        public async Task StatsOther(SocketUser socketUser)
        {
            var account = UserAccounts.GetAccount(socketUser);
            await Context.Channel.SendMessageAsync($"Hey {socketUser.Mention}, You have {account.Size} long sandwhiches and {account.XP} XP.");
        }

        [Command("Daily")]
        public async Task Daily()
        {
            var account = UserAccounts.GetAccount(Context.User);
            account.XP += 200;
            await Context.Channel.SendMessageAsync($"Hey {Context.User.Mention}, You gained 200 XP.");
            UserAccounts.SaveAccounts();
        }

        [Command("Daily")]
        [RequireUserPermission(GuildPermission.MentionEveryone)]
        public async Task Daily(SocketUser socketUser)
        {
            var account = UserAccounts.GetAccount(socketUser);
            account.XP += 200;
            await Context.Channel.SendMessageAsync($"Hey {socketUser.Mention}, You gained 200 XP.");
            UserAccounts.SaveAccounts();
        }



        #region Pick
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
        #endregion

        #region Secret
        [Command("secret")]
        public async Task RevealSecret([Remainder]string arguments = "") 
        {
            ///<summary>
            ///User is not secret owner. Denied.
            /// </summary>
            if (!UserIsSecretOwner((SocketGuildUser)(Context.User), "noob")) 
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
        #endregion

        #region Data Adding
        [Command("Add")]
        public async Task GetData([Remainder]string arguments)
        {
            DataStorage.pairs.Add(arguments.Split('=', ';', '|')[0], arguments.Split('=', ';', '|')[1]);
            DataStorage.SaveData();
            await Context.Channel.SendMessageAsync("DataStorage has " + DataStorage.pairs.Count + " pairs.");
        }
        #endregion

        #region Finding Data
        [Command("Find")]
        public async Task FindData([Remainder]string key)
        {
            await Context.Channel.SendMessageAsync(DataStorage.pairs[key]);
        }
        #endregion

        #region Delete Data
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

        #endregion

        #region View all
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

        #endregion


        public bool UserIsSecretOwner(SocketGuildUser user, string targetRoleName) 
        {
            var result = from r in user.Guild.Roles
                         where r.Name == targetRoleName
                         select r.Id;
            ulong targetRoleID = result.FirstOrDefault();
            if (targetRoleID == 0) return false;
            var targetRole = user.Guild.GetRole(targetRoleID);
            return user.Roles.Contains(targetRole);
        }
    }
}
