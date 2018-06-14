using Discord.Commands;
using Discord;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Pokemon_discord.Modules
{
    public class ServerInfo : ModuleBase<SocketCommandContext> 
    {
        [Command("server")]
        [Summary("Its all about the server bitch")]
        public async Task AllAboutServer()
        {
            var embed = new EmbedBuilder();
            embed.AddField("Server Owner", GuildOwner(Context.Guild), true);
            embed.AddField("Server Created", GuildCreatedDate(Context.Guild), true);
            embed.AddField("Total Roles", RoleCount(Context.Guild),true);
            embed.AddField("Total Channel", ChannelCount(Context.Guild), true);
            embed.AddField("Total members", GuildUserCount(Context.Guild), true);
            embed.AddField("Text channels", TextChannelCount(Context.Guild), true);
            embed.AddField("Total Bots", GuildBotCount(Context.Guild), true);
            embed.AddField("Total Voice Channels", VoiceChannelCount(Context.Guild), true);
            embed.AddField("Total Pirates", GuildHumanCount(Context.Guild));
            embed.WithThumbnailUrl(GuildLogo(Context.Guild));
            embed.WithCurrentTimestamp();
            embed.WithFooter($"Server name: {Context.Guild.Name} | Server ID: {Context.Guild.Id}", GuildLogo(Context.Guild));
            embed.WithColor(((SocketGuildUser)Context.User).Roles.Skip(2).First().Color);

            await Context.Channel.SendMessageAsync("", false, embed.Build());

        }

        public int ChannelCount(SocketGuild socketGuild) => socketGuild.Channels.Count;

        public int VoiceChannelCount(SocketGuild socketGuild) => socketGuild.VoiceChannels.Count;

        public int TextChannelCount(SocketGuild socketGuild) => socketGuild.TextChannels.Count;

        public string GuildOwner(SocketGuild socketGuild) => socketGuild.Owner.ToString();

        public DateTime GuildCreatedDate(SocketGuild socketGuild) => socketGuild.CreatedAt.UtcDateTime;

        public int RoleCount(SocketGuild socketGuild) => socketGuild.Roles.Count;

        public int GuildUserCount(SocketGuild socketGuild) => socketGuild.Users.Count;

        public int GuildBotCount(SocketGuild socketGuild) => (from a in socketGuild.Users
                                                              where a.IsBot == true
                                                              select a).Count();

        public int GuildHumanCount(SocketGuild socketGuild) => GuildUserCount(socketGuild) - GuildBotCount(socketGuild);

        public string GuildLogo(SocketGuild socketGuild) => socketGuild.IconUrl;
    }
}
