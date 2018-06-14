using System;
using System.Threading.Tasks;
using Discord;
using Discord.Net.Providers.WS4Net;
using Discord.WebSocket;
using Pokemon_discord.Core;

namespace Pokemon_discord
{
    internal class Program
    {
        private DiscordSocketClient _client;
        private CommandHandler _handler;

        private static void Main(string[] args)
        {
            new Program().StartAsync().GetAwaiter().GetResult();
        }

        private async Task StartAsync()
        {
            if (string.IsNullOrEmpty(Config.Bot.Token)) return;
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose,
                WebSocketProvider = WS4NetProvider.Instance
            });
            _client.Log += Log;
            _client.Ready += RepeatingTimer.StartTimer;
            await _client.LoginAsync(TokenType.Bot, Config.Bot.Token);
            await _client.StartAsync();
            _handler = new CommandHandler();
            await _handler.InitializeAsync(_client);
            await Task.Delay(-1);
        }

        private async Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.Message);
        }
    }
}