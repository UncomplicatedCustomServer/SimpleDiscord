using System;

namespace SimpleDiscord.Logger
{
    public class ErrorHub(Client discordclient)
    {
        private readonly Client DiscordClient = discordclient;

        public void Throw(string message, bool lethal = false)
        {
            DiscordClient.Logger.Error(message);

            if (lethal)
                throw new Exception($"Internal SimpleDiscord Exception: {message}");
        }
    }
}
