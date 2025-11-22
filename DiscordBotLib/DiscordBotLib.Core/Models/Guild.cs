using System;
using System.Text.Json;

namespace DiscordBotLib.Core.Models
{
    public class Guild
    {
        public ulong id { get; }
        public string name { get; }
        public string icon { get; }
        public ulong ownerid { get; }
        public int membercount { get; }

        public Guild(JsonElement data)
        {
            id = ulong.Parse(data.GetProperty("id").GetString());
            name = data.GetProperty("name").GetString();
            icon = data.GetProperty("icon").GetString();
            ownerid = ulong.Parse(data.GetProperty("owner_id").GetString());
            membercount = data.GetProperty("member_count").GetInt32();
        }

        public override string ToString() => name;
    }
}