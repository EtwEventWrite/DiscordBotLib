using System;
using System.Text.Json;
using System.Collections.Generic;

namespace DiscordBotLib.Core.Models
{
    public class Embed
    {
        public string titlestr { get; }
        public string descriptionstr { get; }
        public string uri { get; }
        public DateTimeOffset? timestamp { get; }
        public int colorstr { get; }
        public EmbedFooter footerembed { get; }
        public EmbedImage imagembed { get; }
        public EmbedAuthor authorembed { get; }
        public List<EmbedField> fieldslist { get; }

        public Embed(JsonElement data)
        {
            if (data.TryGetProperty("title", out var title)) titlestr = title.GetString();
            if (data.TryGetProperty("description", out var desc)) descriptionstr = desc.GetString();
            if (data.TryGetProperty("url", out var url)) uri = url.GetString();
            if (data.TryGetProperty("timestamp", out var ts)) timestamp = DateTimeOffset.Parse(ts.GetString());
            if (data.TryGetProperty("color", out var color)) colorstr = color.GetInt32();
            if (data.TryGetProperty("footer", out var footer)) footerembed = new EmbedFooter(footer);
            if (data.TryGetProperty("image", out var image)) imagembed = new EmbedImage(image);
            if (data.TryGetProperty("author", out var author)) authorembed = new EmbedAuthor(author);
            fieldslist = new List<EmbedField>();
            if (data.TryGetProperty("fields", out var fields))
            {
                foreach (var field in fields.EnumerateArray())
                {
                    fieldslist.Add(new EmbedField(field));
                }
            }
        }
    }

    public class EmbedFooter
    {
        public string text { get; }
        public string iconurl { get; }

        public EmbedFooter(JsonElement data)
        {
            text = data.GetProperty("text").GetString();
            if (data.TryGetProperty("icon_url", out var icon)) iconurl = icon.GetString();
        }
    }

    public class EmbedImage
    {
        public string url { get; }

        public EmbedImage(JsonElement data)
        {
            url = data.GetProperty("url").GetString();
        }
    }

    public class EmbedAuthor
    {
        public string name { get; }
        public string uri { get; }
        public string iconurl { get; }

        public EmbedAuthor(JsonElement data)
        {
            name = data.GetProperty("name").GetString();
            if (data.TryGetProperty("url", out var url)) uri = url.GetString();
            if (data.TryGetProperty("icon_url", out var icon)) iconurl = icon.GetString();
        }
    }

    public class EmbedField
    {
        public string name { get; }
        public string value { get; }
        public bool inline { get; }

        public EmbedField(JsonElement data)
        {
            name = data.GetProperty("name").GetString();
            value = data.GetProperty("value").GetString();
            inline = data.GetProperty("inline").GetBoolean();
        }
    }
}