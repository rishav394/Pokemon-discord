using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Pokemon_discord.ModuleHelper;

namespace Pokemon_discord.Modules
{
    public class Misc : ModuleBase<SocketCommandContext>
    {
        private const string ThumbnailUrl = "https://assets.pokemon.com/static2/_ui/img/global/three-characters.png";

        [Command("rps")]
        public async Task RockPaperScissor(string rps = null)
        {
            if (string.IsNullOrEmpty(rps) || !(rps.Equals("rock") || rps.Equals("paper") || rps.Equals("scissor")))
            {
                await ReplyAsync("You do not have choosen one of the options Rock, Paper or scissor.");
                return;
            }

            string botPick = new[] {"rock", "paper", "scissor"}[new Random().Next(0, 3)];
            var winDictionary =
                new Dictionary<string, string> {{"rock", "scissor"}, {"paper", "rock"}, {"scissor", "paper"}};
            if (botPick == rps) await ReplyAsync("Damn its a draw.");
            else if (winDictionary[rps] == botPick) await ReplyAsync($"Ez win. Your Pick: {rps} bot pick: {botPick}");
            else await ReplyAsync($"Bot cheated and won. Your Pick: {rps} bot pick: {botPick}");
        }

        /// <summary>
        ///     Not a Public command.
        /// </summary>
        /// <returns></returns>
        [Command("Abandon ship")]
        [Alias("kill", "die", "down", "911")]
        public async Task ShutDown()
        {
            await ReplyAsync("Sorry command available only to the Code owner.");
            //Embed embed = new EmbedBuilder().WithColor(Color.Blue).WithTitle("Wait What????")
            //    .WithDescription(Utilities.Get_formatted_alret("ABANDON")).WithThumbnailUrl(AbandonUrl).Build();
            //await Context.Channel.SendMessageAsync("", false, embed);
            //IDMChannel dmChannel = await Context.User.GetOrCreateDMChannelAsync();
            //await dmChannel.SendMessageAsync("Shutdown complete.");
            //await dmChannel.SendMessageAsync("`Environment.Exit(1)`");
            //Environment.Exit(1);
        }

        [Command("Pick")]
        public async Task PickOne([Remainder] string message)
        {
            string[] options = message.Split(new[] {'|', ' '}, StringSplitOptions.RemoveEmptyEntries);
            var r = new Random();
            Embed embed = new EmbedBuilder().WithTitle("Requested by " + Context.User)
                .WithDescription(options[r.Next(0, options.Length)]).WithColor(Color.Blue)
                .WithThumbnailUrl(ThumbnailUrl).Build();
            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("secret")]
        public async Task RevealSecret([Remainder] string arguments = "")
        {
            const string targetRoleName = "Moderator";
            if (!PermissionHelper.IsUserRoleHolder((SocketGuildUser) Context.User, targetRoleName))
            {
                Console.WriteLine("User is not secret owner");
                await Context.Channel.SendMessageAsync(Utilities.Get_formatted_alret("PERMISSION_DENIED",
                    Context.User.Mention));
                return;
            }

            IDMChannel dmChannel = await Context.User.GetOrCreateDMChannelAsync();
            await Context.Channel.SendMessageAsync(Utilities.Get_formatted_alret("CHECK_DM",
                Context.User.Mention)); //Send check_dm in the channel
            Embed embed = new EmbedBuilder().WithTitle("Requested by " + Context.User)
                .WithDescription(Utilities.Get_formatted_alret("SECRET")).WithColor(Color.Blue)
                .WithThumbnailUrl(ThumbnailUrl).Build();
            await dmChannel.SendMessageAsync("", false, embed); // Send embeed in DM
        }
    }
}