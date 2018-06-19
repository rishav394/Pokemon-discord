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
            //_client.MessageDeleted += _client_MessageDeleted;
            await _client.LoginAsync(TokenType.Bot, Config.Bot.Token);
            await _client.StartAsync();
            await _client.SetGameAsync("`@pokemon help` for help");
            Global.Client = _client;
            _handler = new CommandHandler();
            await _handler.InitializeAsync(_client);
            await Task.Delay(-1);
        }
        /*
        private async Task Client_MessageDeleted(Cacheable<IMessage, ulong> messageBefore,
            ISocketMessageChannel arg3)
        {
            try
            {
                if (messageBefore.Value.Channel is ITextChannel kek)
                {
                    var log = await kek.Guild.GetAuditLogAsync(1);
                    var audit = log.ToList();
                    var name = audit[0].Action == ActionType.MessageDeleted ? audit[0].User.Mention : messageBefore.Value.Author.Mention;

                    var embedDel = new EmbedBuilder();
                    embedDel.WithColor(Color.DarkPurple);
                    embedDel.WithTitle($"🗑 Deleted Message in {messageBefore.Value.Channel.Name}");
                    embedDel.WithDescription($"WHO: **{name}**\n" +
                                             $"Mess Author: **{messageBefore.Value.Author}**\n" +
                                             $"Time: **{DateTime.Now.ToLongTimeString()}**\n");
                    embedDel.AddField("Content", $"{messageBefore.Value.Content}");
                    embedDel.AddField("Mess ID", $"{messageBefore.Id}");
                    if (messageBefore.Value.Attachments.Any())
                        embedDel.AddField("attachments", $"URL: {messageBefore.Value.Attachments.FirstOrDefault()?.Url}\n" +
                                                         $"Proxy URL: {messageBefore.Value.Attachments.FirstOrDefault()?.ProxyUrl}");


                    await _client.GetGuild(437628145042980875).GetTextChannel(457630676175552512)
                        .SendMessageAsync("", false, embedDel.Build());
                }
            }
            catch
            {

                //
            }
        }*/

        private async Task InformOppaiDev()
        {
            await _client.GetGuild(437628145042980875).GetTextChannel(457630676175552512)
                .SendMessageAsync($"I was brought online at {DateTime.Now}");
        }

        private async Task Log(LogMessage msg)
        {
            Console.WriteLine(DateTime.Now.ToString("h:mm:ss tt") + " : " + msg.Message);
            await Task.CompletedTask;
        }
    }
}