using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Pokemon_discord.Core.UserAccounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon_discord.Modules
{
    public class Exp: ModuleBase<SocketCommandContext> 
    {
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
            string TargetRole = "Moderator";
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
                if (Misc.IsUserRoleHolder((SocketGuildUser)Context.User, TargetRole))
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

    }
}
