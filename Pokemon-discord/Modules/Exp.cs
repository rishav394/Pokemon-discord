﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Pokemon_discord.Core.UserAccounts;

namespace Pokemon_discord.Modules
{
    public class Exp : ModuleBase<SocketCommandContext>
    {
        [Command("rep")]
        public async Task Repped(SocketUser socketUser)
        {
            if (socketUser == Context.User)
            {
                await ReplyAsync("Dude thats like looking in the mirror and jerkin off. FFS."); //Thanks L
                return;
            }
            var account = UserAccounts.GetAccount(Context.User);
            if (account.RepperList.Contains(socketUser.Id))
            {
                await ReplyAsync("Hey Hey Hey you already did it once.");
                var allTheRepped = from a in Context.Guild.Users where account.RepperList.Contains(a.Id) select a;
                var embed = new EmbedBuilder();
                embed.WithTitle("You have repped all these lads so far");
                embed.WithThumbnailUrl("https://media1.tenor.com/images/b6dff5dd473c0b1b5f6ade724c9434ed/tenor.gif?itemid=4068088");
                embed.WithCurrentTimestamp();
                var temp = 1;
                foreach (var a in allTheRepped)
                {
                    embed.Description += $"\n{temp++.ToString()}. {a.Username}";
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

        [Command("Rep")]
        public async Task Repped()
        {
            var account = UserAccounts.GetAccount(Context.User);
            var allTheRepped = from a in Context.Guild.Users where account.RepperList.Contains(a.Id) select a;
            var embed = new EmbedBuilder();
            embed.WithTitle("You have repped all these lads so far");
            embed.WithThumbnailUrl("https://media1.tenor.com/images/b6dff5dd473c0b1b5f6ade724c9434ed/tenor.gif?itemid=4068088");
            embed.WithCurrentTimestamp();
            var temp = 1;
            foreach (var a in allTheRepped)
            {
                embed.Description += $"\n{temp++.ToString()}. {a.Username}";
            }

            await ReplyAsync("", false, embed.Build());
        }

        [Command("Stats")]
        public async Task StatsOther(SocketUser socketUser = null)
        {
            if (socketUser == null) socketUser = Context.User;
            var account = UserAccounts.GetAccount(socketUser);
            await Context.Channel.SendMessageAsync(
                $"Hey {socketUser.Mention}, You have {account.Size} long sandwhiches and {account.Xp} XP.");
        }

        [Command("Daily")]
        public async Task Daily(SocketUser socketUser = null)
        {
            var targetRole = "Moderator";
            if (socketUser == null)
            {
                socketUser = Context.User;
                var account = UserAccounts.GetAccount(socketUser);
                if (DateTime.UtcNow - account.Dt > TimeSpan.FromDays(1))
                {
                    account.Xp += 200;
                    account.Dt = DateTime.UtcNow;
                    await Context.Channel.SendMessageAsync($"Hey {socketUser.Mention}, You gained 200 XP.");
                    UserAccounts.SaveAccounts();
                }
                else
                {
                    await Context.Channel.SendMessageAsync($"There there you needy child. " +
                        $"{(24 - (DateTime.UtcNow - account.Dt).TotalHours).ToString("N2")} Hours to go.");
                }
            }
            else
            {
                if (Misc.IsUserRoleHolder((SocketGuildUser) Context.User, targetRole))
                {
                    var account = UserAccounts.GetAccount(socketUser);
                    account.Xp += 200;
                    await Context.Channel.SendMessageAsync($"Hey {socketUser.Mention}, You gained 200 XP.");
                    UserAccounts.SaveAccounts();
                }
                else
                {
                    await Context.Channel.SendMessageAsync(
                        $"Mmm sorry {Context.User.Mention}, You need to be a {targetRole} to be able to do so");
                }
            }
        }
    }
}