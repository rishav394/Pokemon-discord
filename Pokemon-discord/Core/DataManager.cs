using Pokemon_discord.Core.UserAccounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace Pokemon_discord.Core
{
    public static class DataManager
    {
        // save all user accounts
        public static void SaveUserAccounts(IEnumerable<UserAccount> accounts,string FilePath)
        {
            string json = JsonConvert.SerializeObject(accounts,Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }

        // get all user accounts
        public static IEnumerable<UserAccount> GetUserAccounts(string FilePath)
        {
            if (!File.Exists(FilePath))
            {
                return null;
            }
            string json = File.ReadAllText(FilePath);
            return JsonConvert.DeserializeObject<IEnumerable<UserAccount>>(json);
        }

        public static bool ExistsFile(string file)
        {
            if (File.Exists(file))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
