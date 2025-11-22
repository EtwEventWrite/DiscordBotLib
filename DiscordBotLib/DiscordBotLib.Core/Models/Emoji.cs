using System;
using System.Text.Json;

namespace DiscordBotLib.Core.Models
{
    public class Emoji
    {
        public ulong? idulg { get; }
        public string name { get; }
        public bool animatedbool { get; }

        public Emoji(JsonElement data)
        {
            if (data.TryGetProperty("id", out var id) && id.ValueKind != JsonValueKind.Null)
                idulg = ulong.Parse(id.GetString());

            name = data.GetProperty("name").GetString();

            if (data.TryGetProperty("animated", out var animated))
                animatedbool = animated.GetBoolean();
            else
                animatedbool = false;
        }

        public override string ToString() => idulg.HasValue ? $"{name}:{idulg}" : name;
    }
}