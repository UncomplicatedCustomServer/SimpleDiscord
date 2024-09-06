using System.Collections.Generic;

namespace SimpleDiscord.Components
{
    public class SocketSendPoll(PollMediaObject question, List<PollAnswer> answers, int duration = 2, bool? allowMultiselect = null)
    {
        public PollMediaObject Question { get; } = question;

        public List<PollAnswer> Answers { get; } = answers;

        public int Duration { get; } = duration;

        public bool? AllowMultiselect { get; } = allowMultiselect;

        public int LayoutType => 1;
    }
}
