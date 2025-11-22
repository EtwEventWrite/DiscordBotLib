using System;
using System.Text.Json;

namespace DiscordBotLib.Core.Models
{
    public class Permission
    {
        public ulong id { get; }
        public PermissionType type { get; }
        public string permissionstring { get; }
        public bool allow { get; }
        public bool deny { get; }

        public Permission(JsonElement data)
        {
            id = ulong.Parse(data.GetProperty("id").GetString());
            type = (PermissionType)data.GetProperty("type").GetInt32();
            permissionstring = data.GetProperty("permission").GetString();
            allow = data.GetProperty("allow").GetBoolean();
            deny = data.GetProperty("deny").GetBoolean();
        }

        public bool haspermission(PermissionFlag flag)
        {
            var permissions = ulong.Parse(permissionstring);
            return ((permissions & (ulong)flag) == (ulong)flag);
        }

        public override string ToString() => $"{type} {id}: {(allow ? "Allow" : "Deny")}";
    }

    public enum PermissionType
    {
        Role = 0,
        Member = 1
    }

    [Flags]
    public enum PermissionFlag : ulong
    {
        CreateInstantInvite = 1UL << 0,
        KickMembers = 1UL << 1,
        BanMembers = 1UL << 2,
        Administrator = 1UL << 3,
        ManageChannels = 1UL << 4,
        ManageGuild = 1UL << 5,
        AddReactions = 1UL << 6,
        ViewAuditLog = 1UL << 7,
        PrioritySpeaker = 1UL << 8,
        Stream = 1UL << 9,
        ViewChannel = 1UL << 10,
        SendMessages = 1UL << 11,
        SendTTSMessages = 1UL << 12,
        ManageMessages = 1UL << 13,
        EmbedLinks = 1UL << 14,
        AttachFiles = 1UL << 15,
        ReadMessageHistory = 1UL << 16,
        MentionEveryone = 1UL << 17,
        UseExternalEmojis = 1UL << 18,
        ViewGuildInsights = 1UL << 19,
        Connect = 1UL << 20,
        Speak = 1UL << 21,
        MuteMembers = 1UL << 22,
        DeafenMembers = 1UL << 23,
        MoveMembers = 1UL << 24,
        UseVAD = 1UL << 25,
        ChangeNickname = 1UL << 26,
        ManageNicknames = 1UL << 27,
        ManageRoles = 1UL << 28,
        ManageWebhooks = 1UL << 29,
        ManageEmojisAndStickers = 1UL << 30,
        UseSlashCommands = 1UL << 31,
        RequestToSpeak = 1UL << 32,
        ManageEvents = 1UL << 33,
        ManageThreads = 1UL << 34,
        CreatePublicThreads = 1UL << 35,
        CreatePrivateThreads = 1UL << 36,
        UseExternalStickers = 1UL << 37,
        SendMessagesInThreads = 1UL << 38,
        UseEmbeddedActivities = 1UL << 39,
        ModerateMembers = 1UL << 40
    }
}