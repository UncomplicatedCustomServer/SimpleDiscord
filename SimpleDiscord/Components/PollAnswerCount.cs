namespace SimpleDiscord.Components
{
    public class PollAnswerCount(int id, int count, bool meVoted)
    {
        public int Id { get; } = id;

        public int Count { get; } = count;

        public bool MeVoted { get; } = meVoted;
    }
}
