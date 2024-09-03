using System;

namespace SimpleDiscord.Logger
{
#nullable enable
    public static class Log
    {
        public static string Prefix { get; set; } = "SimpleDiscord";

        public static Func<LogEntry, bool>? SubstituteLogHandler { get; private set; }

        public static void SendLog(LogEntry entry)
        {
            if (SubstituteLogHandler is not null)
                if (!SubstituteLogHandler(entry))
                    return;

            Console.ResetColor();

            Console.ForegroundColor = entry.Level switch
            {
                LogLevel.Debug => ConsoleColor.Green,
                LogLevel.Info => ConsoleColor.Cyan,
                LogLevel.Warn => ConsoleColor.DarkYellow,
                LogLevel.Error => ConsoleColor.Red,
                _ => ConsoleColor.White
            };

            Console.WriteLine($"[{entry.TimeOffset.ToLocalTime()}] [{Prefix}] [{entry.Level.ToString().ToUpper()}] {entry.Content}");

            Console.ResetColor();
        }

        public static void SendLog(string message, LogLevel level) => SendLog(new(message, level));

        public static void Debug(string message) => SendLog(message, LogLevel.Debug);

        public static void Info(string message) => SendLog(message, LogLevel.Info);

        public static void Warn(string message) => SendLog(message, LogLevel.Warn);

        public static void Error(string message) => SendLog(message, LogLevel.Error);
    }
}
