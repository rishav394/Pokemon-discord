using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord.Commands;

namespace Pokemon_discord.Modules
{
    public class Emoji : ModuleBase<SocketCommandContext>
    {
        [Command("Emotify")]
        [Alias("emoji", "emotion", "say", "emote")]
        public async Task Emotify([Remainder] string args)
        {
            string[] convertorArray = {"zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine"};
            args = args.ToLower();
            var convertedText = "";
            var mentionFound = false;
            foreach (char c in args)
            {
                if (c == '<') mentionFound = true;
                if (c == '>') mentionFound = false;
                if (mentionFound)
                {
                    convertedText += c;
                    continue;
                }

                if (char.IsLetter(c)) convertedText += $":regional_indicator_{c}:";
                else if (char.IsDigit(c)) convertedText += $":{convertorArray[(int) char.GetNumericValue(c)]}:";
                else if (c == '.') convertedText += " :record_button: ";
                else if (c == '?') convertedText += ":question: ";
                else convertedText += c;
                if (char.IsWhiteSpace(c)) convertedText += "  ";
            }

            await ReplyAsync(convertedText);
        }

        [Command("emotion2")]
        [Summary("A better method by @mylorik")]
        public async Task NewEmotify([Remainder] string args)
        {
            string[] convertorArray = {"zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine"};
            var pattern = new Regex("^[a-zA-Z]*$", RegexOptions.Compiled);
            args = args.ToLower();
            var convertedText = "";
            foreach (char c in args)
                if (c.ToString() == "\\")
                    convertedText += "\\";
                else if (c.ToString() == "\n") convertedText += "\n";
                else if (pattern.IsMatch(c.ToString())) convertedText += $":regional_indicator_{c}:";
                else if (char.IsDigit(c)) convertedText += $":{convertorArray[(int) char.GetNumericValue(c)]}:";
                else convertedText += c;
            await ReplyAsync(convertedText);
        }
    }
}