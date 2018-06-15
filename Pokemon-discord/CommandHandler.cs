using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

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

            if (s.Content.Contains("you finished your digging! Use"))
                await _client.GetGuild(437628145042980875)
                    .GetTextChannel(437635106887172098)
                    .SendMessageAsync($"<@334750493085794304>, someone just finished digging. Fuck em hard.");

//            if (s.Content.Contains("┬─┬ ノ( ゜-゜ノ)"))
//            {
//                await context.Channel.SendMessageAsync("Not Happenin dude.");
//                await context.Channel.SendMessageAsync("(╯°□°）╯︵ ┻━┻");
//            }

            if (msg.HasStringPrefix(Config.Bot.CmdPrefix, ref argPos) ||
                msg.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                //await s.Channel.SendMessageAsync($"{s.Author.Mention} hi");
                //This enables auto reply on every single command
                //See - https://stackoverflow.com/questions/50813554/make-discord-bot-to-reply-same-message-for-any-command
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
    }
}