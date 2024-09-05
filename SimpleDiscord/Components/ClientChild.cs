namespace SimpleDiscord.Components
{
    public class ClientChild()
    {
        internal Client Client { get; private set; }

        internal void SetClient(Client client) => Client = client;
    }
}
