using Newtonsoft.Json;

namespace SimpleDiscord.Components
{
#nullable enable
    public class SocketInteractionResponse
    {
        public int Type { get; }

        public object? Data { get; }

        [JsonConstructor]
        public SocketInteractionResponse(int type, object? data)
        {
            Type = type;
            Data = data;
        }

        public SocketInteractionResponse(SocketInteractionResponse self)
        {
            Type = self.Type;
            Data = self.Data;
        }
    }
}
