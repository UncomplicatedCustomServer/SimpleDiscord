namespace SimpleDiscord.Components
{
    public class PollResults(bool isFinalized, PollAnswerCount[] answerCounts)
    {
        public bool IsFinalized { get; } = isFinalized;

        public PollAnswerCount[] AnswerCounts { get; } = answerCounts;
    }
}
