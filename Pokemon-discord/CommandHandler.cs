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

        public async Task InitialiseAsync(DiscordSocketClient discordSocketClient)
        {
            _client = discordSocketClient;
            _service = new CommandService();
            await _service.AddModulesAsync(Assembly.GetEntryAssembly());
            _client.MessageReceived += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketMessage s)
        {
            var msg = s as SocketUserMessage;
            if (msg == null)
            {
                return;
            }
            var context = new SocketCommandContext(_client, msg);

            int argPos = 0;
            if(msg.HasStringPrefix(Config.bot.cmdPrefix,ref argPos) || msg.HasMentionPrefix(_client.CurrentUser,ref argPos))
            {
                var result = await _service.ExecuteAsync(context, argPos);
                if (!result.IsSuccess && result.Error!= CommandError.UnknownCommand) 
                {
                    Console.WriteLine(result.ErrorReason);
                }
            }
        }
    }
}
