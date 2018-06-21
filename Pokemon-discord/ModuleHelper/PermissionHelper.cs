using System.Linq;
using Discord;
using Discord.WebSocket;

namespace Pokemon_discord.ModuleHelper
{
    internal class PermissionHelper
    {
        public static bool HasPermission(SocketGuildUser socketGuildUser, GuildPermission guildPermission)
        {
            return socketGuildUser.GuildPermissions.ToList().Contains(guildPermission);
        }

        public static bool IsUserRoleHolder(SocketGuildUser user, string targetRoleName)
        {
            ulong roleId = (from r in user.Guild.Roles where r.Name == targetRoleName select r.Id).FirstOrDefault();
            return user.Roles.Contains(user.Guild.GetRole(roleId));
        }
    }
}