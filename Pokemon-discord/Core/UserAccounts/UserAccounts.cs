using System;
using System.Collections.Generic;
using System.Linq;
using Discord.WebSocket;

namespace Pokemon_discord.Core.UserAccounts
{
    public static class UserAccounts
    {
        private static List<UserAccount> _accounts;
        private const string AccountsFile = "Resources/accounts.json";
        private static SocketUser _su;

        static UserAccounts()
        {
            if (DataManager.ExistsFile(AccountsFile))
            {
                _accounts = DataManager.GetUserAccounts(AccountsFile).ToList();
            }
            else
            {
                _accounts = new List<UserAccount>();
                SaveAccounts();
            }
        }

        public static void SaveAccounts()
        {
            DataManager.SaveUserAccounts(_accounts, AccountsFile);
        }

        public static UserAccount GetAccount(SocketUser user)
        {
            _su = user;
            return GetOrCreateUserAccount(user.Id);
        }

        public static bool AccountExists(SocketUser user)
        {
            ulong id = user.Id;
            IEnumerable<UserAccount> result = from a in _accounts where a.Id == id select a;
            UserAccount foundAccount = result.FirstOrDefault();
            if (foundAccount == null) return false;
            return true;
        }

        private static UserAccount GetOrCreateUserAccount(ulong id)
        {
            IEnumerable<UserAccount> result = from a in _accounts where a.Id == id select a;
            UserAccount foundAccount = result.FirstOrDefault();
            if (foundAccount == null) foundAccount = CreateUserAccount(id);
            return foundAccount;
        }

        private static UserAccount CreateUserAccount(ulong id)
        {
            var newAccount = new UserAccount
            {
                Id = id,
                Size = 0,
                Xp = 100,
                RepperList = new List<ulong>(),
                Countem = 0,
                WarningCount = 0,
                UnmuteDateTime = new Dictionary<ulong, DateTime> {{((SocketGuildUser) _su).Guild.Id, DateTime.Now}}
            };
            _accounts.Add(newAccount);
            SaveAccounts();
            return newAccount;
        }
    }
}