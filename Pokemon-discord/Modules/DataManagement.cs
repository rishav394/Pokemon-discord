using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon_discord.Modules
{
    public class DataManagement:ModuleBase<SocketCommandContext>
    {
        private const string ThumbnailUrl = "https://assets.pokemon.com/static2/_ui/img/global/three-characters.png";

        [Command("Add")]
        public async Task GetData([Remainder]string arguments)
        {
            DataStorage.pairs.Add(arguments.Split('=', ';', '|')[0], arguments.Split('=', ';', '|')[1]);
            DataStorage.SaveData();
            await Context.Channel.SendMessageAsync("DataStorage has " + DataStorage.pairs.Count + " pairs.");
        }

        [Command("Find")]
        public async Task FindData([Remainder]string key)
        {
            await Context.Channel.SendMessageAsync(DataStorage.pairs[key]);
        }

        [Command("Delete")]
        public async Task DeleteData([Remainder]string key = null)
        {
            if (key == null)
            {
                DataStorage.pairs.Clear();
            }
            else
            {
                Console.WriteLine($"Trying to remove {key}");
                DataStorage.pairs.Remove(key);
            }
            DataStorage.SaveData();
            await Context.Channel.SendMessageAsync("DataStorage has " + DataStorage.pairs.Count + " pairs.");
        }

        [Command("View")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task PrintData()
        {
            string desc = null;
            foreach (KeyValuePair<string, string> x in DataStorage.pairs)
            {
                desc += x.Key + " " + x.Value + "\n";
            }
            desc += "\b";
            var embed = new EmbedBuilder()
            .WithTitle("Requested by " + Context.User.Mention)
            .WithColor(Color.Gold)
            .WithDescription(desc)
            .WithThumbnailUrl(ThumbnailUrl)
            .WithImageUrl("")
            .Build();

            await Context.Channel.SendMessageAsync("", false, embed);
        }
    }
}
