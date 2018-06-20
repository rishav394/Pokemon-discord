using System;
using System.Collections.Generic;
using System.Linq;
using Discord.WebSocket;

namespace Pokemon_discord.Core.UserAccounts
{
    public static class UserAccounts
    {
        public static List<UserAccount> Accounts;
        private static readonly string AccountsFile = "Resources/accounts.json";
        public static SocketUser su;

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
            su = user;
            return GetOrCreateUserAccount(user.Id);
        }

        public static bool AccountExists(SocketUser user)
        {
            ulong id = user.Id;
            IEnumerable<UserAccount> result = from a in Accounts where a.Id == id select a;
            UserAccount foundAccount = result.FirstOrDefault();
            if (foundAccount == null) return false;
            return true;
        }

        public static UserAccount GetOrCreateUserAccount(ulong id)
        {
            IEnumerable<UserAccount> result = from a in Accounts where a.Id == id select a;
            UserAccount foundAccount = result.FirstOrDefault();
            if (foundAccount == null)
            {
                foundAccount = CreateUserAccount(id);
            }
            //else
            //{
            //    if (!foundAccount.UnmuteDateTime.ContainsKey(((SocketGuildUser)su).Guild.Id)) 
            //        foundAccount.UnmuteDateTime.Add(((SocketGuildUser)su).Guild.Id, DateTime.MinValue);
            //    SaveAccounts();
            //}
           
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
                Countem = 0,
                WarningCount = 0,
                UnmuteDateTime = new Dictionary<ulong, DateTime> { { ((SocketGuildUser) su).Guild.Id, DateTime.Now } }
            };
            Accounts.Add(newAccount);
            SaveAccounts();
            return newAccount;
        }
    }
}