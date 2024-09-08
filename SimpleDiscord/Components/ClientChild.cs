using Newtonsoft.Json;

namespace SimpleDiscord.Components
{
    public class ClientChild() : DisposableElement
    {
        [JsonIgnore]
        internal Client Client { get; private set; }

        internal void SetClient(Client client) => Client = client;
    }
}
