using System.Collections.Generic;
using Discord.WebSocket;
using System.Linq;

namespace Pokemon_discord.Core.UserAccounts
{
    public static class UserAccounts
    {
        public static List<UserAccount> accounts = null;

        private static readonly string AccountsFile = "Resources/accounts.json";

        static UserAccounts()
        {
            if (DataManager.ExistsFile(AccountsFile))
            {
                accounts = DataManager.GetUserAccounts(AccountsFile).ToList();
            }
            else
            {
                accounts = new List<UserAccount>();
                SaveAccounts();
            }
        }

        public static void SaveAccounts() => DataManager.SaveUserAccounts(accounts, AccountsFile);

        public static UserAccount GetAccount(SocketUser user)
        {
            return GetOrCreateUserAccount(user.Id);
        }

        public static UserAccount GetOrCreateUserAccount(ulong ID)
        {
            var result = from a in accounts
                         where a.ID == ID
                         select a;

            var FoundAccount = result.FirstOrDefault();
            if (FoundAccount == null)
            {
                FoundAccount = CreateUserAccount(ID);
            }
            return FoundAccount;
        }

        public static UserAccount CreateUserAccount(ulong id)
        {
            var newAccount = new UserAccount
            {
                ID = id,
                Size = 0,
                XP = 100,
                RepperList = new List<ulong>(),
                Countem = 0
            };
            accounts.Add(newAccount);
            SaveAccounts();
            return newAccount;
        }
    }
}
