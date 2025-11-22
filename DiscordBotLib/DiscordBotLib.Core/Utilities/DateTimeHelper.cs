using System;

namespace DiscordBotLib.Core.Utilities
{
    internal class DateTimeHelper
    {
        private static readonly DateTimeOffset _discordepoch = new DateTimeOffset(2015, 1, 1, 0, 0, 0, TimeSpan.Zero);

        public static DateTimeOffset FromSnowflake(ulong snowflake)
        {
            long timestamp = (long)(snowflake >> 22);
            return _discordepoch.AddMilliseconds(timestamp);
        }

        public static ulong ToSnowflake(DateTimeOffset timestamp)
        {
            long milliseconds = (long)(timestamp - _discordepoch).TotalMilliseconds;
            return (ulong)(milliseconds << 22);
        }

        public static string ToDiscordTimestamp(DateTimeOffset timestamp, string format = "f")
        {
            long unixtimestamp = timestamp.ToUnixTimeSeconds();
            return $"<t:{unixtimestamp}:{format}>";
        }

        public static string ToRelativeDiscordTimestamp(DateTimeOffset timestamp)
        {
            long unixtimestamp = timestamp.ToUnixTimeSeconds();
            return $"<t:{unixtimestamp}:R>";
        }

        public static DateTimeOffset FromDiscordTimestamp(string discordtimestamp)
        {
            if (discordtimestamp.StartsWith("<t:") && discordtimestamp.EndsWith(">"))
            {
                string timestampStr = discordtimestamp.Substring(3, discordtimestamp.Length - 4);
                string[] parts = timestampStr.Split(':');
                if (parts.Length > 0 && long.TryParse(parts[0], out long unixtimestamp))
                {
                    return DateTimeOffset.FromUnixTimeSeconds(unixtimestamp);
                }
            }
            throw new ArgumentException("Invalid Discord timestamp format");
        }

        public static TimeSpan CalculateUptime(DateTimeOffset starttime)
        {
            return DateTimeOffset.UtcNow - starttime;
        }

        public static string FormatUptime(TimeSpan uptime)
        {
            if (uptime.TotalDays >= 1)
                return $"{(int)uptime.TotalDays}d {uptime.Hours}h {uptime.Minutes}m {uptime.Seconds}s";
            else if (uptime.TotalHours >= 1)
                return $"{uptime.Hours}h {uptime.Minutes}m {uptime.Seconds}s";
            else if (uptime.TotalMinutes >= 1)
                return $"{uptime.Minutes}m {uptime.Seconds}s";
            else
                return $"{uptime.Seconds}s";
        }

        public static bool IsOlderThan(DateTimeOffset timestamp, TimeSpan age)
        {
            return DateTimeOffset.UtcNow - timestamp > age;
        }

        public static DateTimeOffset GetDiscordEpoch()
        {
            return _discordepoch;
        }
    }
}