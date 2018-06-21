using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Pokemon_discord.Core.UserAccounts;
using Pokemon_discord.ModuleHelper;

namespace Pokemon_discord.Modules
{
    public class Moderation : ModuleBase<SocketCommandContext>
    {
        [Command("Bye")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task LeaveTask()
        {
            await ReplyAsync("But But But What did i do :(");
            await Context.Guild.LeaveAsync();
        }

        [Command("Mute")]
        [Summary("Ask a noob to shadafakup")]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        [RequireUserPermission(GuildPermission.MuteMembers)]
        public async Task MuteRealTask(SocketGuildUser socketGuildUser, string forTime = null)
        {
            //check if bot isnt muting admin
            if (socketGuildUser == Context.Guild.GetUser(454001308555280418))
            {
                await ReplyAsync("Not happening kiddo.");
                return;
            }

            if (PermissionHelper.HasPermission(socketGuildUser, GuildPermission.Administrator))
            {
                await ReplyAsync($"The force is stong with {socketGuildUser.Username}. Administrators cant be muted.");
                return;
            }

            //Fetching the roles
            SocketRole theMutedRole =
                (from a in socketGuildUser.Guild.Roles where a.Name == "muted" select a).FirstOrDefault();
            if (theMutedRole == null)
            {
                await socketGuildUser.Guild.CreateRoleAsync("muted", GuildPermissions.None, Color.DarkRed, true);
                theMutedRole = (from a in socketGuildUser.Guild.Roles where a.Name == "muted" select a)
                    .FirstOrDefault();
            }

            //add role
            await socketGuildUser.AddRoleAsync(theMutedRole);

            //Add role to channel
            var perms = new OverwritePermissions(sendMessages: PermValue.Deny);
            foreach (SocketTextChannel socketTextChannel in socketGuildUser.Guild.TextChannels)
                await socketGuildUser.Guild.GetTextChannel(socketTextChannel.Id)
                    .AddPermissionOverwriteAsync(theMutedRole, perms);
            await ReplyAsync($"Allright {socketGuildUser.Mention} was muted from all channels.");

            //TODO: add time dependency
        }

        [Command("unmute")]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        [RequireUserPermission(GuildPermission.MuteMembers)]
        public async Task UnmuTask(SocketGuildUser socketGuildUser)
        {
            if (PermissionHelper.IsUserRoleHolder(socketGuildUser, "muted"))
            {
                SocketRole targetRole = (from a in socketGuildUser.Guild.Roles where a.Name == "muted" select a)
                    .FirstOrDefault();
                await socketGuildUser.RemoveRoleAsync(targetRole);
                await ReplyAsync($"Ok {socketGuildUser.Mention} was unmuted.");
            }
            else
            {
                await ReplyAsync($"{socketGuildUser.Username} is not muted");
            }
        }

        [Command("purge")]
        [Alias("prune", "clear")]
        [Summary("Deletes the specified amount of messages.")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
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
        [RequireUserPermission(GuildPermission.BanMembers)]
        public async Task BanTask(SocketGuildUser socketGuildUser, int days = 0,
            [Remainder] string reason = "No reason")
        {
            if (socketGuildUser == socketGuildUser.Guild.GetUser(454001308555280418))
            {
                await ReplyAsync("Not happening kiddo.");
                return;
            }

            await Context.Guild.AddBanAsync(socketGuildUser, days, reason);
            string printableDays = days == 0 ? "Infinite" : days.ToString();
            await ReplyAsync($"{socketGuildUser.Mention} was banned for {reason} for {printableDays} days.");
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

            await socketGuildUser.KickAsync(reason);
            await ReplyAsync($"{socketGuildUser.Mention} was kicked for {reason}.");
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
            if (account.WarningCount == 2)
                await ReplyAsync(
                    $"Now is the time. Beg, beg for a `{Config.Bot.PrefixDictionary[Context.Guild.Id]}Reset Warning`.");
            if (account.WarningCount >= 3)
            {
                account.WarningCount = 0;
                await KickTask(socketGuildUser, "User exceeded total no of warnings");
            }

            UserAccounts.SaveAccounts();
        }

        [Command("Reset Warning")]
        [Alias("resetw", "Reset_Warning", "Reset_warnings", "reset warnings")]
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

        [Command("prefix")]
        public async Task PrefiTask([Remainder] string newPrefix = null)
        {
            if (string.IsNullOrEmpty(newPrefix))
            {
                await ReplyAsync($"My current prefix is `{Config.Bot.PrefixDictionary[Context.Guild.Id]}` |" +
                                 $" Use `{Config.Bot.PrefixDictionary[Context.Guild.Id]}help` for help");
            }
            else
            {
                if (!PermissionHelper.HasPermission((SocketGuildUser) Context.User, GuildPermission.Administrator))
                {
                    await ReplyAsync($"You need to be an Administrator to make a change to prefix.");
                    return;
                }

                Config.Bot.PrefixDictionary[Context.Guild.Id] = newPrefix;
                Config.SavePrefix();
                await ReplyAsync(
                    $"Aight my prefix has been changed to `{Config.Bot.PrefixDictionary[Context.Guild.Id]}`. You can always mention me though.");
            }
        }
    }
}