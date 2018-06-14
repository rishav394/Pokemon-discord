using System;
using System.Collections.Generic;

namespace Pokemon_discord.Core.UserAccounts
{
    public class UserAccount
    {
        public ulong ID { get; set; }

        public uint Size { get; set; }

        public uint XP { get; set; }

        public DateTime Dt { get; set; }

        public List<ulong> RepperList { get; set; }

        public int Countem { get; set; }

    }
}
