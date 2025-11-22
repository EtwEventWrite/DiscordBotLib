using System;
using System.IO;
using System.Text;
using System.Text.Json;

namespace DiscordBotLib.Core.Networking
{
    public class JsonSerializer
    {
        private readonly JsonSerializerOptions options;

        public JsonSerializer()
        {
            options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false,
                IgnoreNullValues = true
            };
        }

        public string Serialize(object obj)
        {
            return System.Text.Json.JsonSerializer.Serialize(obj, options);
        }

        public T Deserialize<T>(string json)
        {
            return System.Text.Json.JsonSerializer.Deserialize<T>(json, options);
        }

        public object Deserialize(string json, Type type)
        {
            return System.Text.Json.JsonSerializer.Deserialize(json, type, options);
        }

        public T DeserializeFile<T>(string filepath)
        {
            string jsoncontent = File.ReadAllText(filepath, Encoding.UTF8);
            return Deserialize<T>(jsoncontent);
        }

        public void SerializeToFile(object obj, string filepath)
        {
            string json = Serialize(obj);
            File.WriteAllText(filepath, json, Encoding.UTF8);
        }

        public JsonElement DeserializeDynamic(string json)
        {
            using (JsonDocument doc = JsonDocument.Parse(json))
            {
                return doc.RootElement.Clone();
            }
        }

        public bool IsValidJson(string json)
        {
            try
            {
                using (JsonDocument.Parse(json))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}