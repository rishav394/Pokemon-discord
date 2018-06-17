using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Pokemon_discord.Core.UserAccounts;

namespace Pokemon_discord.Modules
{
    public class Moderation : ModuleBase<SocketCommandContext>
    {
        [Command("purge")]
        [Alias("prune", "clear")]
        [Summary("Deletes the specified amount of messages.")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(ChannelPermission.ManageMessages)]
        public async Task PurgeChat(int amount)
        {
            IEnumerable<IMessage> messages = await Context.Channel.GetMessagesAsync(amount + 1).FlattenAsync();
            await ((ITextChannel) Context.Channel).DeleteMessagesAsync(messages);
            const int delay = 3000;
            IUserMessage m = await ReplyAsync($"I have deleted {amount} messages for ya. :)");
            await Task.Delay(delay);
            await m.DeleteAsync();
        }

        [Command("Ban")]
        [RequireBotPermission(GuildPermission.BanMembers)]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task BanTask(SocketGuildUser socketGuildUser, int days = 0,
            [Remainder] string reason = "No reason")
        {
            if (socketGuildUser == socketGuildUser.Guild.GetUser(454001308555280418))
            {
                await ReplyAsync("Not happening kiddo.");
                return;
            }

            string printableDays = days == 0 ? "Infinite" : days.ToString();
            await ReplyAsync($"{socketGuildUser.Mention} was banned for {reason} for {printableDays} days.");
            await Context.Guild.AddBanAsync(socketGuildUser, days, reason);
        }

        [Command("kick")]
        [RequireBotPermission(GuildPermission.KickMembers)]
        [RequireUserPermission(GuildPermission.KickMembers)]
        private async Task KickTask(SocketGuildUser socketGuildUser, [Remainder] string reason = "No reason")
        {
            if (socketGuildUser == socketGuildUser.Guild.GetUser(454001308555280418))
            {
                await ReplyAsync("Not happening kiddo.");
                return;
            }

            await ReplyAsync($"{socketGuildUser.Mention} was kicked for {reason}.");
            await socketGuildUser.KickAsync(reason);
        }

        [Command("warn")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        public async Task WarnTask(SocketGuildUser socketGuildUser, [Remainder] string reason = "No reason")
        {
            if (socketGuildUser == socketGuildUser.Guild.GetUser(454001308555280418))
            {
                await ReplyAsync("Not happening kiddo.");
                return;
            }

            UserAccount account = UserAccounts.GetAccount(socketGuildUser);
            account.WarningCount++;
            await ReplyAsync($"All right {socketGuildUser.Mention} was warned for {reason}.");
            await ReplyAsync($"Total no of warnings = {account.WarningCount}");
            if (account.WarningCount >= 3)
            {
                account.WarningCount = 0;
                await KickTask(socketGuildUser, "User exceeded total no of warnings");
            }

            UserAccounts.SaveAccounts();
        }

        [Command("Reset Warning")]
        [Alias("resetw")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task ReserWarnignTask(SocketGuildUser socketGuildUser)
        {
            UserAccount account = UserAccounts.GetAccount(socketGuildUser);
            account.WarningCount = 0;
            await ReplyAsync(
                $"Aight {socketGuildUser.Mention} thank the angels. Your number of infractions have been reset.");
            UserAccounts.SaveAccounts();
        }

        [Command("Permissions")]
        [Alias("p", "permission")]
        public async Task DoiTask(SocketUser iGuildUser = null)
        {
            if (iGuildUser == null) iGuildUser = Context.User;
            var embedBuilder = new EmbedBuilder {Description = "", Color = Color.Blue};
            var temp = 1;
            foreach (GuildPermission guildPermission in ((SocketGuildUser) iGuildUser).GuildPermissions.ToList())
                embedBuilder.Description += temp++ + ". " + guildPermission + "\n";
            embedBuilder.WithTitle($"{iGuildUser.Username}'s permissions");
            embedBuilder.WithFooter($"Requested by {Context.User.Username}");
            await ReplyAsync("", false, embedBuilder.Build());
        }
    }
}