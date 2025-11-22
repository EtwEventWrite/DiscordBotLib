using System;
using System.Collections.Generic;
using System.Linq;
using DiscordBotLib.Core.Models;

namespace DiscordBotLib.DiscordBotLib.Core.Utilities
{
    internal class PermissionCalculator
    {
        public static bool HasPermission(ulong userpermissions, PermissionFlag permission)
        {
            return (userpermissions & (ulong)permission) == (ulong)permission;
        }

        public static ulong AddPermission(ulong currentpermissions, PermissionFlag permission)
        {
            return currentpermissions | (ulong)permission;
        }

        public static ulong RemovePermission(ulong currentpermissions, PermissionFlag permission)
        {
            return currentpermissions & ~(ulong)permission;
        }

        public static ulong CalculateChannelPermissions(ulong guildpermissions, List<Permission> channeloverwrites, ulong everyonepermissions)
        {
            if (HasPermission(guildpermissions, PermissionFlag.Administrator))
            {
                return (ulong)GetAllPermissions();
            }
            ulong basepermissions = guildpermissions;
            if (channeloverwrites != null)
            {
                basepermissions = ApplyPermissionOverwritesToBits(basepermissions, channeloverwrites);
            }
            basepermissions &= everyonepermissions;
            return basepermissions;
        }

        public static ulong CalculateUserPermissions(User user, Guild guild, Channel channel)
        {
            if (guild == null) return 0;
            if (user.id == guild.ownerid)
            {
                return (ulong)GetAllPermissions();
            }
            ulong permissions = 0;
            var everyonerole = guild.roles?.FirstOrDefault(r => r.id == guild.id);
            if (everyonerole != null)
            {
                permissions = everyonerole.permissions;
            }
            if (user.roles != null && guild.roles != null)
            {
                foreach (var roleId in user.roles)
                {
                    var role = guild.roles.FirstOrDefault(r => r.id == roleId);
                    if (role != null)
                    {
                        permissions |= role.permissions;
                    }
                }
            }
            if (HasPermission(permissions, PermissionFlag.Administrator))
            {
                return (ulong)GetAllPermissions();
            }
            if (channel != null && channel.permissionoverwrites != null)
            {
                permissions = ApplyPermissionOverwritesToBits(permissions, channel.permissionoverwrites);
            }

            return permissions;
        }

        private static ulong ApplyPermissionOverwritesToBits(ulong basepermissions, List<Permission> overwrites)
        {
            ulong permissions = basepermissions;
            var everyoneoverwrite = overwrites.FirstOrDefault(o => o.type == PermissionType.Role && o.id == 0);
            if (everyoneoverwrite != null)
            {
                permissions = ApplySingleOverwrite(permissions, everyoneoverwrite);
            }
            var roleOverwrites = overwrites.Where(o => o.type == PermissionType.Role && o.id != 0);
            foreach (var overwrite in roleOverwrites)
            {
                permissions = ApplySingleOverwrite(permissions, overwrite);
            }
            var userOverwrites = overwrites.Where(o => o.type == PermissionType.Member);
            foreach (var overwrite in userOverwrites)
            {
                permissions = ApplySingleOverwrite(permissions, overwrite);
            }

            return permissions;
        }

        private static ulong ApplySingleOverwrite(ulong permissions, Permission overwrite)
        {
            if (ulong.TryParse(overwrite.permissionstring, out ulong permissionbits))
            {
                if (overwrite.allow)
                {
                    permissions |= permissionbits;
                }
                if (overwrite.deny)
                {
                    permissions &= ~permissionbits;
                }
            }
            return permissions;
        }

        public static bool CanUserSendMessages(User user, Guild guild, Channel channel)
        {
            var permissions = CalculateUserPermissions(user, guild, channel);
            return HasPermission(permissions, PermissionFlag.SendMessages);
        }

        public static bool CanUserManageMessages(User user, Guild guild, Channel channel)
        {
            var permissions = CalculateUserPermissions(user, guild, channel);
            return HasPermission(permissions, PermissionFlag.ManageMessages);
        }

        public static bool CanUserKickMembers(User user, Guild guild)
        {
            var permissions = CalculateUserPermissions(user, guild, null);
            return HasPermission(permissions, PermissionFlag.KickMembers);
        }

        public static bool CanUserBanMembers(User user, Guild guild)
        {
            var permissions = CalculateUserPermissions(user, guild, null);
            return HasPermission(permissions, PermissionFlag.BanMembers);
        }

        public static bool CanUserViewChannel(User user, Guild guild, Channel channel)
        {
            var permissions = CalculateUserPermissions(user, guild, channel);
            return HasPermission(permissions, PermissionFlag.ViewChannel);
        }

        public static PermissionFlag GetAllPermissions()
        {
            return (PermissionFlag)~0UL;
        }

        public static string GetPermissionName(PermissionFlag permission)
        {
            return permission.ToString();
        }

        public static List<PermissionFlag> GetPermissionFlags(ulong permissions)
        {
            var flags = new List<PermissionFlag>();
            foreach (PermissionFlag flag in Enum.GetValues(typeof(PermissionFlag)))
            {
                if (HasPermission(permissions, flag))
                {
                    flags.Add(flag);
                }
            }
            return flags;
        }
    }
}