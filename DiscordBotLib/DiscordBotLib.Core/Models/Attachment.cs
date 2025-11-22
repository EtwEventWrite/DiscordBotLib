using System;
using System.Text.Json;

namespace DiscordBotLib.Core.Models
{
    public class Attachment
    {
        public ulong id { get; }
        public string filename { get; }
        public string url { get; }
        public string proxyurl { get; }
        public int size { get; }
        public int? widthint { get; }
        public int? heightint { get; }

        public Attachment(JsonElement data)
        {
            id = ulong.Parse(data.GetProperty("id").GetString());
            filename = data.GetProperty("filename").GetString();
            url = data.GetProperty("url").GetString();
            proxyurl = data.GetProperty("proxy_url").GetString();
            size = data.GetProperty("size").GetInt32();

            if (data.TryGetProperty("width", out var width)) widthint = width.GetInt32();
            if (data.TryGetProperty("height", out var height)) heightint = height.GetInt32();
        }

        public override string ToString() => filename;
    }
}