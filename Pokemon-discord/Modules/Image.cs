using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using NReco.ImageGenerator;
using ImageFormat = NReco.ImageGenerator.ImageFormat;

namespace Pokemon_discord.Modules
{
    public class Image : ModuleBase<SocketCommandContext>
    {
        [Command("Tenor")]
        public async Task GetTenor([Remainder] string args)
        {
            var rand = new Random();
            var embed = new EmbedBuilder();
            embed.WithImageUrl(TenorApi.TinyUrl(args));
            embed.WithColor(new Color(rand.Next(0, 256), rand.Next(0, 256), rand.Next(0, 256)));
            embed.WithFooter($"Requested by {Context.User.Username}");
            await ReplyAsync("", false, embed.Build());
        }

        [Command("Big Tenor")]
        public async Task GetBigTenor([Remainder] string args)
        {
            var rand = new Random();
            var embed = new EmbedBuilder();
            embed.WithImageUrl(TenorApi.BigUrl(args));
            embed.WithColor(new Color(rand.Next(0, 256), rand.Next(0, 256), rand.Next(0, 256)));
            embed.WithFooter($"Requested by {Context.User.Username}");
            await ReplyAsync("", false, embed.Build());
        }

        [Command("Hello")]
        [Alias("Greet")]
        public async Task ImageShit([Remainder] string args = "")
        {
            const string css = "<style>\n	h1{\n		color: rgb(27,82,122);\n	}\n</style>\n";
            string html = $"<h1>{args}</h1>";
            if (args == "") html = $"<h1>Hello {Context.User.Username}</h1>";
            byte[] jpegBytes =
                new HtmlToImageConverter {Width = 200, Height = 70}.GenerateImage(css + html, ImageFormat.Jpeg);
            await Context.Channel.SendFileAsync(new MemoryStream(jpegBytes), "Hello.jpeg");
        }
    }
}