using System;
using System.Threading.Tasks;
using Discord;
using Discord.Net.Providers.WS4Net;
using Discord.WebSocket;

namespace Pokemon_discord
{
    internal class Program
    {
        private DiscordSocketClient _client;
        private CommandHandler _handler;

        private static void Main()
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
            _client.Ready += InformOppaiDev;
            await _client.LoginAsync(TokenType.Bot, Config.Bot.Token);
            await _client.StartAsync();
            Global.Client = _client;
            _handler = new CommandHandler();
            await _handler.InitializeAsync(_client);
            await Task.Delay(-1);
        }

        private async Task InformOppaiDev()
        {
            await _client.GetGuild(437628145042980875).GetTextChannel(457630676175552512)
                .SendMessageAsync($"I was brought online at {DateTime.Now}");
        }

        private async Task Log(LogMessage msg)
        {
            Console.WriteLine(DateTime.Now.ToString("h:mm:ss tt") + " : " + msg.Message);
        }
    }
}