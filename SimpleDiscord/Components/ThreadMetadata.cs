namespace SimpleDiscord.Components
{
    public class ThreadMetadata(bool archived, int autoArchiveDuration, string archiveTimestamp, bool locked, bool? invitable, string createTimestamp)
    {
        public bool Archived { get; } = archived;

        public int AutoArchiveDuration { get; } = autoArchiveDuration;

        public string ArchiveTimestamp { get; } = archiveTimestamp;

        public bool Locked { get; } = locked;

        public bool? Invitable { get; } = invitable;

        public string CreateTimestamp { get; } = createTimestamp;
    }
}
