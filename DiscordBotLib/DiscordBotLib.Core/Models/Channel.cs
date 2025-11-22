using System;
using System.Text.Json;

namespace DiscordBotLib.Core.Models
{
    public class Channel
    {
        public ulong id { get; }
        public ChannelType type { get; }
        public ulong? guildid { get; }
        public string name { get; }
        public string topic { get; }

        public Channel(JsonElement data)
        {
            id = ulong.Parse(data.GetProperty("id").GetString());
            type = (ChannelType)data.GetProperty("type").GetInt32();
            name = data.GetProperty("name").GetString();
            topic = data.GetProperty("topic").GetString();
            if (data.TryGetProperty("guild_id", out var guildId) && guildId.ValueKind != JsonValueKind.Null)
                guildid = ulong.Parse(guildId.GetString());
        }

        public override string ToString() => $"{name} ({type})";
    }

    public enum ChannelType
    {
        GUILD_TEXT = 0,
        DM = 1,
        GUILD_VOICE = 2,
        GROUP_DM = 3,
        GUILD_CATEGORY = 4,
        GUILD_ANNOUNCEMENT = 5,
        ANNOUNCEMENT_THREAD = 10,
        PUBLIC_THREAD = 11,
        PRIVATE_THREAD = 12,
        GUILD_STAGE_VOICE = 13,
        GUILD_DIRECTORY = 14,
        GUILD_FORUM = 15
    }
}