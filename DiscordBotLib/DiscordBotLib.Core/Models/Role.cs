using System;
using System.Text.Json;

namespace DiscordBotLib.Core.Models
{
    public class Role
    {
        public ulong id { get; }
        public string name { get; }
        public int color { get; }
        public int position { get; }
        public string permissions { get; }
        public bool managed { get; }
        public bool mentionable { get; }

        public Role(JsonElement data)
        {
            id = ulong.Parse(data.GetProperty("id").GetString());
            name = data.GetProperty("name").GetString();
            color = data.GetProperty("color").GetInt32();
            position = data.GetProperty("position").GetInt32();
            permissions = data.GetProperty("permissions").GetString();
            managed = data.GetProperty("managed").GetBoolean();
            mentionable = data.GetProperty("mentionable").GetBoolean();
        }

        public override string ToString() => name;
    }
}