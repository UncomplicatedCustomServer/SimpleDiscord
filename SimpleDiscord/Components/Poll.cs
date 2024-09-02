namespace SimpleDiscord.Components
{
    public class Poll(PollMediaObject question, PollAnswer[] answers, int duration, bool? allowMultiselect = null)
    {
        public PollMediaObject Question { get; } = question;

        public PollAnswer[] Answers { get; } = answers;

        public int Duration { get; } = duration;

        public bool? AllowMultiselect { get; } = allowMultiselect;

        public int LayoutType => 1;
    }
}
