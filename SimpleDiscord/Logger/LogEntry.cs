using System;

namespace SimpleDiscord.Logger
{
    public class LogEntry(string content, LogLevel level = LogLevel.None)
    {
        public LogLevel Level { get; } = level;

        public string Content { get; } = content;

        public long Time { get; } = DateTimeOffset.Now.ToUnixTimeSeconds();

        public DateTimeOffset TimeOffset { get; } = DateTimeOffset.Now;
    }
}
