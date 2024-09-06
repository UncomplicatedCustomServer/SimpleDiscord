using System.Collections.Generic;
using System.Linq;

namespace SimpleDiscord.Components
{
    public class Member : SocketMember
    {
        public Guild Guild { get; }

        public new List<Role> Roles { get; } = [];

        public Member(Guild guild, SocketMember socketInstance) : base(socketInstance)
        {
            Guild = guild;
            
            foreach (long role in socketInstance.Roles)
                Roles.Add(Guild.GetRole(role));
        }

        public void AddRole(Role role, string reason = null) => AddRole(role.Id, reason);

        public void AddRole(long role, string reason = null)
        {
            if (Roles.Any(r => r.Id == role))
                return;

            Guild.Client.RestHttp.MemberAddRole(this, role, reason);
        }

        public void RemoveRole(Role role, string reason = null) => RemoveRole(role.Id, reason);

        public void RemoveRole(long role, string reason = null)
        {
            if (!Roles.Any(r => r.Id == role))
                return;

            Guild.Client.RestHttp.MemberRemoveRole(this, role, reason);
        }

        public void Kick(string reason = null) => Guild.Client.RestHttp.MemberKick(this, reason);

        public void Ban(string reason = null, int deleteMessageSeconds = 0) => Guild.Client.RestHttp.MemberBan(this, reason, deleteMessageSeconds);

        public void SyncUser() => User = SocketUser.List.FirstOrDefault(u => u.Id == User.Id);
    }
}
