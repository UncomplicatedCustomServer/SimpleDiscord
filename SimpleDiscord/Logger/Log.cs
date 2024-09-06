using System;

namespace SimpleDiscord.Logger
{
#nullable enable
    public class Log(ClientConfig config)
    {
        private readonly ClientConfig Config = config;

        public string Prefix { get; set; } = "SimpleDiscord";

        public Func<LogEntry, bool>? SubstituteLogHandler { get; private set; }

        public void SendLog(LogEntry entry)
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

        public void SendLog(string message, LogLevel level) => SendLog(new(message, level));

        public void Silent(string message)
        {
            if (Config.SystemLogs)
                SendLog(message, LogLevel.None);
        }

        public void Debug(string message)
        {
            if (Config.Debug)
                SendLog(message, LogLevel.Debug);
        }

        public void Info(string message) => SendLog(message, LogLevel.Info);

        public void Warn(string message) => SendLog(message, LogLevel.Warn);

        public void Error(string message) => SendLog(message, LogLevel.Error);
    }
}
