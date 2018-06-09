using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon_discord.Core.UserAccounts
{
    public class UserAccount
    {
        public ulong ID { get; set; }

        public uint Size { get; set; }

        public uint XP { get; set; }

        public DateTime Dt { get; set; }

    }
}
