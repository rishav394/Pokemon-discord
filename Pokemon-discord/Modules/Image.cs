using Discord.Commands;
using NReco.ImageGenerator;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Pokemon_discord.Modules
{
    public class Image:ModuleBase<SocketCommandContext>
    {

        [Command("Hello")]
        [Alias("Image")]
        public async Task ImageShit([Remainder]string args = "")
        {
            string css = "<style>\n	h1{\n		color: rgb(27,82,122);\n	}\n</style>\n";
            string html = $"<h1>{args}</h1>";
            if (args == "")
            {
                html = $"<h1>Hello {Context.User.Username}</h1>";   
            }
            var jpegBytes = new HtmlToImageConverter
            {
                Width = 200,
                Height = 70
            }.GenerateImage(css + html, NReco.ImageGenerator.ImageFormat.Jpeg);
            await Context.Channel.SendFileAsync(new MemoryStream(jpegBytes), "Hello.jpeg");
        }

    }
}
