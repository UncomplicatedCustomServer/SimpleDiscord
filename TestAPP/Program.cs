using SimpleDiscord;
using SimpleDiscord.Components;
using SimpleDiscord.Events;
using SimpleDiscord.Events.Attribute;
using SimpleDiscord.Gateway;
using System.Threading;
using System.Threading.Tasks;

namespace TestAPP
{
    internal class Program
    {
        public static Bot Bot { get; private set; }

        static void Main(string[] args)
        {
            Bot = new Bot();

            Bot.Connect();
            Thread.Sleep(-1);
        }
    }
}
