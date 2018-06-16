using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using NReco.ImageGenerator;

namespace Pokemon_discord.Modules
{
    public class Image : ModuleBase<SocketCommandContext>
    {

        [Command("Tenor")]
        public async Task GetTenor([Remainder] string args)
        {
            Random Rand = new Random();
            var embed = new EmbedBuilder();
            embed.WithImageUrl(TenorAPI.TinyUrl(args));
            embed.WithColor(new Color(Rand.Next(0, 256), Rand.Next(0, 256), Rand.Next(0, 256)));
            embed.WithFooter($"Requested by {Context.User.Username}");
            await ReplyAsync("", false, embed.Build());
        }

        [Command("Big Tenor")]
        public async Task GetBigTenor([Remainder]string args)
        {
            Random Rand = new Random();
            var embed = new EmbedBuilder();
            embed.WithImageUrl(TenorAPI.BigUrl(args));
            embed.WithColor(new Color(Rand.Next(0, 256), Rand.Next(0, 256), Rand.Next(0, 256)));
            embed.WithFooter($"Requested by {Context.User.Username}");
            await ReplyAsync("", false, embed.Build());
        }



        [Command("Hello")]
        [Alias("Greet")]
        public async Task ImageShit([Remainder] string args = "")
        {
            var css = "<style>\n	h1{\n		color: rgb(27,82,122);\n	}\n</style>\n";
            var html = $"<h1>{args}</h1>";
            if (args == "") html = $"<h1>Hello {Context.User.Username}</h1>";

            var jpegBytes =
                new HtmlToImageConverter {Width = 200, Height = 70}.GenerateImage(css + html,
                    NReco.ImageGenerator.ImageFormat.Jpeg);
            await Context.Channel.SendFileAsync(new MemoryStream(jpegBytes), "Hello.jpeg");
        }
    }
}