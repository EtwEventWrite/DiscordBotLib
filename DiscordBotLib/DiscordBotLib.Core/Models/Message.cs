using System;
using System.Text.Json;

namespace DiscordBotLib.Core.Models
{
    public class Message
    {
        public ulong id { get; }
        public string content { get; }
        public ulong channelid { get; }
        public ulong? guildid { get; }
        public User author { get; }
        public DateTimeOffset timestamp { get; }

        public Message(JsonElement data)
        {
            id = ulong.Parse(data.GetProperty("id").GetString());
            content = data.GetProperty("content").GetString();
            channelid = ulong.Parse(data.GetProperty("channel_id").GetString());
            if (data.TryGetProperty("guild_id", out var guildId) && guildId.ValueKind != JsonValueKind.Null) guildid = ulong.Parse(guildId.GetString());
            author = new User(data.GetProperty("author"));
            timestamp = DateTimeOffset.Parse(data.GetProperty("timestamp").GetString());
        }
    }
}