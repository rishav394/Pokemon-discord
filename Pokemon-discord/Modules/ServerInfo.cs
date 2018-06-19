using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Pokemon_discord.Modules
{
    public class ServerInfo : ModuleBase<SocketCommandContext>
    {
        [Command("server")]
        [Summary("Its all about the server")]
        public async Task AllAboutServer()
        {
            var embed = new EmbedBuilder();
            embed.AddField("Server Owner", GuildOwner(Context.Guild), true);
            embed.AddField("Server Created", GuildCreatedDate(Context.Guild), true);
            embed.AddField("Total Roles", RoleCount(Context.Guild), true);
            embed.AddField("Total Channel", ChannelCount(Context.Guild), true);
            embed.AddField("Total members", GuildUserCount(Context.Guild), true);
            embed.AddField("Text channels", TextChannelCount(Context.Guild), true);
            embed.AddField("Total Bots", GuildBotCount(Context.Guild), true);
            embed.AddField("Total Voice Channels", VoiceChannelCount(Context.Guild), true);
            embed.AddField("Total Pirates", GuildHumanCount(Context.Guild), true);
            embed.AddField("Server Location", Context.Guild.VoiceRegionId, true);
            embed.WithThumbnailUrl(GuildLogo(Context.Guild));
            embed.WithCurrentTimestamp();
            embed.WithFooter($"Server name: {Context.Guild.Name} | Server ID: {Context.Guild.Id}",
                GuildLogo(Context.Guild));
            embed.WithColor(((SocketGuildUser) Context.User).Roles.Skip(2).First().Color);
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        private int ChannelCount(SocketGuild socketGuild)
        {
            return socketGuild.Channels.Count;
        }

        private int VoiceChannelCount(SocketGuild socketGuild)
        {
            return socketGuild.VoiceChannels.Count;
        }

        private int TextChannelCount(SocketGuild socketGuild)
        {
            return socketGuild.TextChannels.Count;
        }

        private string GuildOwner(SocketGuild socketGuild)
        {
            return socketGuild.Owner.ToString();
        }

        private DateTime GuildCreatedDate(SocketGuild socketGuild)
        {
            return socketGuild.CreatedAt.UtcDateTime;
        }

        private int RoleCount(SocketGuild socketGuild)
        {
            return socketGuild.Roles.Count;
        }

        private int GuildUserCount(SocketGuild socketGuild)
        {
            return socketGuild.Users.Count;
        }

        private int GuildBotCount(SocketGuild socketGuild)
        {
            return (from a in socketGuild.Users where a.IsBot select a).Count();
        }

        private int GuildHumanCount(SocketGuild socketGuild)
        {
            return GuildUserCount(socketGuild) - GuildBotCount(socketGuild);
        }

        private string GuildLogo(SocketGuild socketGuild)
        {
            return socketGuild.IconUrl;
        }
    }
}