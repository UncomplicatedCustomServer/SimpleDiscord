namespace SimpleDiscord.Gateway.Events
{
    internal interface IGatewayEvent
    {
        public string RawData { get; }

        public void Init();
    }
}
