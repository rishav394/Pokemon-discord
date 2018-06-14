using Discord.Commands;
using System.Threading.Tasks;

namespace Pokemon_discord.Modules
{
    public class Emoji : ModuleBase<SocketCommandContext> 
    {
        [Command("Emotify")]
        [Alias("emoji","emotion")]
        public async Task Emotify([Remainder]string args)
        {
            string[] ConvertorArray = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
            args = args.ToLower();
            string ConvertedText = "";
            foreach (char c in args)
            {
                if (char.IsLetter(c))
                    ConvertedText += $":regional_indicator_{c}:";
                else if (char.IsDigit(c))
                {
                    ConvertedText += $":{ConvertorArray[(int)char.GetNumericValue(c)]}:";
                }
                else
                {
                    ConvertedText += c;
                }
            }
            await ReplyAsync(ConvertedText);
        }
    }
}
