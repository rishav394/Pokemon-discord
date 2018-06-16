using System.Collections.Generic;
using System.Linq;
using Discord.WebSocket;

namespace Pokemon_discord.Core.UserAccounts
{
    public static class UserAccounts
    {
        public static List<UserAccount> Accounts;
        private static readonly string AccountsFile = "Resources/accounts.json";

        static UserAccounts()
        {
            if (DataManager.ExistsFile(AccountsFile))
            {
                Accounts = DataManager.GetUserAccounts(AccountsFile).ToList();
            }
            else
            {
                Accounts = new List<UserAccount>();
                SaveAccounts();
            }
        }

        public static void SaveAccounts()
        {
            DataManager.SaveUserAccounts(Accounts, AccountsFile);
        }

        public static UserAccount GetAccount(SocketUser user)
        {
            return GetOrCreateUserAccount(user.Id);
        }

        public static UserAccount GetOrCreateUserAccount(ulong id)
        {
            var result = from a in Accounts where a.Id == id select a;
            var foundAccount = result.FirstOrDefault();
            if (foundAccount == null) foundAccount = CreateUserAccount(id);
            return foundAccount;
        }

        public static UserAccount CreateUserAccount(ulong id)
        {
            var newAccount = new UserAccount
            {
                Id = id,
                Size = 0,
                Xp = 100,
                RepperList = new List<ulong>(),
                Countem = 0
            };
            Accounts.Add(newAccount);
            SaveAccounts();
            return newAccount;
        }
    }
}