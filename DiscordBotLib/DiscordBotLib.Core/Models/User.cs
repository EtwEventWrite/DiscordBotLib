using System;
using System.Text.Json;

namespace DiscordBotLib.Core.Models
{
    public class User
    {
        public ulong id { get; }
        public string username { get; }
        public string discriminator { get; }
        public string avatar { get; }
        public bool bot { get; }

        public User(JsonElement data)
        {
            id = ulong.Parse(data.GetProperty("id").GetString());
            username = data.GetProperty("username").GetString();
            discriminator = data.GetProperty("discriminator").GetString();
            avatar = data.GetProperty("avatar").GetString();
            bot = data.GetProperty("bot").GetBoolean();
        }

        public override string ToString() => $"{username}#{discriminator}";
    }
}
