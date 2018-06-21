using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Pokemon_discord.Core.UserAccounts;

namespace Pokemon_discord.Core
{
    public static class DataManager
    {
        // save all user accounts
        public static void SaveUserAccounts(IEnumerable<UserAccount> accounts, string filePath)
        {
            string json = JsonConvert.SerializeObject(accounts, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        // get all user accounts
        public static IEnumerable<UserAccount> GetUserAccounts(string filePath)
        {
            if (!File.Exists(filePath)) return null;
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<IEnumerable<UserAccount>>(json);
        }

        public static bool ExistsFile(string file)
        {
            return File.Exists(file);
        }
    }
}