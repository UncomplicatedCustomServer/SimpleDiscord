using Newtonsoft.Json;

namespace SimpleDiscord.Components
{
    public class ClientChild()
    {
        [JsonIgnore]
        internal Client Client { get; private set; }

        internal void SetClient(Client client) => Client = client;
    }
}
