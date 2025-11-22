using System;
using System.Text.Json;
using System.Linq;

namespace DiscordBotLib.Core.Models
{
    public class Activity
    {
        public string name { get; }
        public ActivityType type { get; }
        public string url { get; }
        public DateTimeOffset? createdat { get; }
        public ActivityTimestamps timestamps { get; }
        public string applicationid { get; }
        public string details { get; }
        public string state { get; }
        public ActivityEmoji emoji { get; }
        public ActivityParty party { get; }
        public ActivityAssets assets { get; }
        public ActivitySecrets secrets { get; }
        public bool instance { get; }
        public int? flags { get; }

        public Activity(JsonElement data)
        {
            name = data.GetProperty("name").GetString();
            type = (ActivityType)data.GetProperty("type").GetInt32();
            if (data.TryGetProperty("url", out var urlvar) && urlvar.ValueKind != JsonValueKind.Null)
                url = urlvar.GetString();
            if (data.TryGetProperty("created_at", out var createdatvar))
                createdat = DateTimeOffset.FromUnixTimeMilliseconds(createdatvar.GetInt64());
            if (data.TryGetProperty("timestamps", out var timestampsvar))
                timestamps = new ActivityTimestamps(timestampsvar);
            if (data.TryGetProperty("application_id", out var appidvar))
                applicationid = appidvar.GetString();
            if (data.TryGetProperty("details", out var detailsvar))
                details = detailsvar.GetString();
            if (data.TryGetProperty("state", out var statevar))
                state = statevar.GetString();
            if (data.TryGetProperty("emoji", out var emojivar))
                emoji = new ActivityEmoji(emojivar);
            if (data.TryGetProperty("party", out var partyvar))
                party = new ActivityParty(partyvar);
            if (data.TryGetProperty("assets", out var assetsvar))
                assets = new ActivityAssets(assetsvar);
            if (data.TryGetProperty("secrets", out var secretsvar))
                secrets = new ActivitySecrets(secretsvar);
            if (data.TryGetProperty("instance", out var instancevar))
                instance = instancevar.GetBoolean();
            if (data.TryGetProperty("flags", out var flagsvar))
                flags = flagsvar.GetInt32();
        }

        public override string ToString() => $"{name} ({type})";
    }

    public enum ActivityType
    {
        Playing = 0,
        Streaming = 1,
        Listening = 2,
        Watching = 3,
        Custom = 4,
        Competing = 5
    }

    public class ActivityTimestamps
    {
        public DateTimeOffset? start { get; }
        public DateTimeOffset? end { get; }

        public ActivityTimestamps(JsonElement data)
        {
            if (data.TryGetProperty("start", out var startvar))
                start = DateTimeOffset.FromUnixTimeMilliseconds(startvar.GetInt64());
            if (data.TryGetProperty("end", out var endvar))
                end = DateTimeOffset.FromUnixTimeMilliseconds(endvar.GetInt64());
        }
    }

    public class ActivityEmoji
    {
        public string name { get; }
        public ulong? id { get; }
        public bool animated { get; }

        public ActivityEmoji(JsonElement data)
        {
            name = data.GetProperty("name").GetString();
            if (data.TryGetProperty("id", out var idvar) && idvar.ValueKind != JsonValueKind.Null)
                id = ulong.Parse(idvar.GetString());
            if (data.TryGetProperty("animated", out var animatedvar))
                animated = animatedvar.GetBoolean();
        }
    }

    public class ActivityParty
    {
        public string id { get; }
        public int[] size { get; } 

        public ActivityParty(JsonElement data)
        {
            if (data.TryGetProperty("id", out var idvar))
                id = idvar.GetString();
            if (data.TryGetProperty("size", out var sizevar))
            {
                var sizearray = sizevar.EnumerateArray().ToArray();
                size = new int[] { sizearray[0].GetInt32(), sizearray[1].GetInt32() };
            }
        }
    }

    public class ActivityAssets
    {
        public string largeimage { get; }
        public string largetext { get; }
        public string smallimage { get; }
        public string smalltext { get; }

        public ActivityAssets(JsonElement data)
        {
            if (data.TryGetProperty("large_image", out var largeimgvar))
                largeimage = largeimgvar.GetString();
            if (data.TryGetProperty("large_text", out var largetextvar))
                largetext = largetextvar.GetString();
            if (data.TryGetProperty("small_image", out var smallimgvar))
                smallimage = smallimgvar.GetString();
            if (data.TryGetProperty("small_text", out var smalltextvar))
                smalltext = smalltextvar.GetString();
        }
    }

    public class ActivitySecrets
    {
        public string join { get; }
        public string spectate { get; }
        public string match { get; }

        public ActivitySecrets(JsonElement data)
        {
            if (data.TryGetProperty("join", out var joinvar))
                join = joinvar.GetString();
            if (data.TryGetProperty("spectate", out var spectatevar))
                spectate = spectatevar.GetString();
            if (data.TryGetProperty("match", out var matchvar))
                match = matchvar.GetString();
        }
    }
}