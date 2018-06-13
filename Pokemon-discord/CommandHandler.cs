using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using Discord.WebSocket;
using Discord.Commands;
using System.Reflection;


namespace Pokemon_discord
{
    class CommandHandler
    {
        DiscordSocketClient _client;
        CommandService _service;

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
            int argPos = 0;
            if (msg.HasStringPrefix(Config.bot.cmdPrefix, ref argPos)
                || msg.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {

                //await s.Channel.SendMessageAsync($"{s.Author.Mention} hi");
                //This enables auto reply on every single command
                //See - https://stackoverflow.com/questions/50813554/make-discord-bot-to-reply-same-message-for-any-command

                var result = await _service.ExecuteAsync(context, argPos);
                if (!result.IsSuccess)
                {
                    Console.WriteLine(result.ErrorReason);
                    var embed = new Discord.EmbedBuilder();
                    embed.WithTitle("Error: Couldn't execute command ");
                    embed.WithDescription(result.ErrorReason);
                    embed.WithColor(new Discord.Color(0x00ff00));
                    embed.WithThumbnailUrl(context.User.GetAvatarUrl(Discord.ImageFormat.Auto, 64));
                    embed.WithTimestamp(DateTimeOffset.UtcNow);
                    await context.Channel.SendMessageAsync("", false, embed.Build());
                }
            }
        }
    }
}