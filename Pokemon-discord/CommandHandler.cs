using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Pokemon_discord.Core.UserAccounts;

namespace Pokemon_discord
{
    internal class CommandHandler
    {
        private DiscordSocketClient _client;
        private CommandService _service;

        public async Task InitializeAsync(DiscordSocketClient client)
        {
            _client = client;
            _service = new CommandService();
            await _service.AddModulesAsync(Assembly.GetEntryAssembly());
            _client.MessageReceived += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketMessage s)
        {
            if (!(s is SocketUserMessage msg)) return;
            var context = new SocketCommandContext(_client, msg);
            var argPos = 0;

            if (s.Content.Contains("(╯°□°）╯︵ ┻━┻"))
            {
                await context.Channel.SendMessageAsync("Not Happenin dude.");
                await context.Channel.SendMessageAsync("┬─┬ ノ( ゜-゜ノ)");
            }

            if (CheckIfMuted(context.User))
            {
                await context.Message.DeleteAsync();
                return;
            }

            if (!Config.Bot.PrefixDictionary.ContainsKey(context.Guild.Id))
            {
                Config.Bot.PrefixDictionary.Add(context.Guild.Id, "~");
                Config.SavePrefix();
            } 

            if (msg.HasStringPrefix(Config.Bot.PrefixDictionary[context.Guild.Id], ref argPos) ||
                msg.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                IResult result = await _service.ExecuteAsync(context, argPos);
                if (!result.IsSuccess)
                {
                    Console.WriteLine(result.ErrorReason);
                    var embed = new EmbedBuilder();
                    embed.WithTitle("Error: Couldn't execute command ");
                    embed.WithDescription(result.ErrorReason);
                    embed.WithColor(new Color(0x00ff00));
                    embed.WithThumbnailUrl(context.User.GetAvatarUrl(ImageFormat.Auto, 64));
                    embed.WithTimestamp(DateTimeOffset.UtcNow);
                    await context.Channel.SendMessageAsync("", false, embed.Build());
                }
            }
        }

        private bool CheckIfMuted(SocketUser contextUser)
        {
            if (!UserAccounts.AccountExists(contextUser))
            {
                return false;
            }
            UserAccount account = UserAccounts.GetAccount(contextUser);

            return account.UnmuteDateTime.ContainsKey(((SocketGuildUser)contextUser).Guild.Id)
                   && account.UnmuteDateTime[((SocketGuildUser)contextUser).Guild.Id] > DateTime.Now;
        }
    }
}