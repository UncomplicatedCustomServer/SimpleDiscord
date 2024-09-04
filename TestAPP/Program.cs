using SimpleDiscord;
using SimpleDiscord.Components;
using SimpleDiscord.Events;
using SimpleDiscord.Events.Attribute;
using SimpleDiscord.Gateway;
using System.Runtime.InteropServices;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TestAPP
{
    internal class Program
    {
        public static Bot Bot { get; private set; }

        static void Main(string[] args)
        {
            handler = new ConsoleEventDelegate(ConsoleEventCallback);
            SetConsoleCtrlHandler(handler, true);

            Bot = new Bot();

            Bot.Connect();
            Thread.Sleep(-1);
        }

        static bool ConsoleEventCallback(int eventType)
        {
            if (eventType == 2)
            {
                Console.WriteLine("Console window closing, death imminent");
            }

            Bot.DiscordClient.Disconnect();
            return false;
        }

        static ConsoleEventDelegate handler;   // Keeps it from getting garbage collected
                                               // Pinvoke
        private delegate bool ConsoleEventDelegate(int eventType);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);
    }
}
