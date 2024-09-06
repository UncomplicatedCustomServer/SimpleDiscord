using Newtonsoft.Json;

namespace SimpleDiscord.Components
{
#nullable enable
    public class SocketPoll
    {
        public PollMediaObject Question { get; }

        public PollAnswer[] Answers { get; }

        public string? Expiry { get; }

        public bool AllowMultiselect { get; } 

        public int LayoutType { get; }

        public PollResults? Results { get; }

        [JsonConstructor]
        public SocketPoll(PollMediaObject question, PollAnswer[] answers, string? expiry, bool allowMultiselect, int layoutType, PollResults? results)
        {
            Question = question;
            Answers = answers;
            Expiry = expiry;
            AllowMultiselect = allowMultiselect;
            LayoutType = layoutType;
            Results = results;
        }

        public SocketPoll(SocketPoll self)
        {
            Question = self.Question;
            Answers = self.Answers;
            Expiry = self.Expiry;
            AllowMultiselect = self.AllowMultiselect;
            LayoutType = self.LayoutType;
            Results = self.Results;
        }
    }
}
