using System.Threading.Tasks;
using Discord.Commands;

namespace Pokemon_discord.Modules
{
    public class Emoji : ModuleBase<SocketCommandContext>
    {
        [Command("Emotify")]
        [Alias("emoji", "emotion")]
        public async Task Emotify([Remainder] string args)
        {
            string[] convertorArray = {"zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine"};
            args = args.ToLower();
            var convertedText = "";
            foreach (var c in args)
            {
                if (char.IsLetter(c)) convertedText += $":regional_indicator_{c}:";
                else if (char.IsDigit(c)) convertedText += $":{convertorArray[(int) char.GetNumericValue(c)]}:";
                else convertedText += c;
                if (char.IsWhiteSpace(c)) convertedText += "  ";
            }

            await ReplyAsync(convertedText);
        }
    }
}