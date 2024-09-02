namespace SimpleDiscord.Components
{
#nullable enable
    public class PollAnswer(int answerId, PollMediaObject pollMedia)
    {
        public int AnswerId { get; } = answerId;

        public PollMediaObject PollMedia { get; } = pollMedia;
    }
}
