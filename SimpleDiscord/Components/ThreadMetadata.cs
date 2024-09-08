using Newtonsoft.Json;

namespace SimpleDiscord.Components
{
    public class ThreadMetadata(bool archived, int autoArchiveDuration, string archiveTimestamp, bool locked, bool? invitable, string createTimestamp)
    {
        public bool Archived { get; } = archived;

        [JsonProperty("auto_archive_duration")]
        public int AutoArchiveDuration { get; } = autoArchiveDuration;

        [JsonProperty("archive_timestamp")]
        public string ArchiveTimestamp { get; } = archiveTimestamp;

        public bool Locked { get; } = locked;

        public bool? Invitable { get; } = invitable;

        [JsonProperty("create_timestamp")]
        public string CreateTimestamp { get; } = createTimestamp;
    }
}
