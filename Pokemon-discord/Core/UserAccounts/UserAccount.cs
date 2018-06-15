using System;
using System.Collections.Generic;

namespace Pokemon_discord.Core.UserAccounts
{
    public class UserAccount
    {
        public ulong Id { get; set; }                   //SocketUser.id
        public uint Size { get; set; }                  //Sandwhich Size
        public uint Xp { get; set; }                    //XP
        public DateTime Dt { get; set; }                //A date time variale for Daily handling
        public List<ulong> RepperList { get; set; }     //Who all have u repped so far
        public int Countem { get; set; }                //How many times did u rep someone
    }
}