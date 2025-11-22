using System;

namespace DiscordBotLib.DiscordBotLib.Core.Utilities
{
    internal class LogManager
    {
        public enum LogLevel
        {
            Debug,
            Info,
            Warning,
            Error,
            Critical
        }

        private readonly string _loggername;
        private static readonly object _lockobject = new object();

        public LogManager(string loggername)
        {
            _loggername = loggername;
        }

        public void Log(LogLevel level, string message, Exception exception = null)
        {
            lock (_lockobject)
            {
                var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                var logmessage = $"[{timestamp}] [{level.ToString().ToUpper()}] [{_loggername}] {message}";
                if (exception != null)
                {
                    logmessage += $"\nException: {exception.Message}\nStack Trace: {exception.StackTrace}";
                }
                Console.WriteLine(logmessage);

                // TODO: Add file logging, event system, or other outputs here
            }
        }

        public void Debug(string message)
        {
            Log(LogLevel.Debug, message);
        }

        public void Info(string message)
        {
            Log(LogLevel.Info, message);
        }

        public void Warning(string message, Exception exception = null)
        {
            Log(LogLevel.Warning, message, exception);
        }

        public void Error(string message, Exception exception = null)
        {
            Log(LogLevel.Error, message, exception);
        }

        public void Critical(string message, Exception exception = null)
        {
            Log(LogLevel.Critical, message, exception);
        }

        public static LogManager CreateLogger(string name)
        {
            return new LogManager(name);
        }

        public static LogManager CreateLogger(Type type)
        {
            return new LogManager(type.FullName);
        }
    }
}