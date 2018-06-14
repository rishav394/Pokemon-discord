using System.IO;
using System.Threading.Tasks;
using Discord.Commands;
using NReco.ImageGenerator;

namespace Pokemon_discord.Modules
{
    public class Image : ModuleBase<SocketCommandContext>
    {
        [Command("Hello")]
        [Alias("Image")]
        public async Task ImageShit([Remainder] string args = "")
        {
            var css = "<style>\n	h1{\n		color: rgb(27,82,122);\n	}\n</style>\n";
            var html = $"<h1>{args}</h1>";
            if (args == "") html = $"<h1>Hello {Context.User.Username}</h1>";

            var jpegBytes =
                new HtmlToImageConverter {Width = 200, Height = 70}.GenerateImage(css + html,
                    ImageFormat.Jpeg);
            await Context.Channel.SendFileAsync(new MemoryStream(jpegBytes), "Hello.jpeg");
        }
    }
}