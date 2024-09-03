using System.Threading.Tasks;

namespace SimpleDiscord.Networking
{
#nullable enable
    public interface ISyncableElement
    {
        public Task<object>? Sync();
    }
}
