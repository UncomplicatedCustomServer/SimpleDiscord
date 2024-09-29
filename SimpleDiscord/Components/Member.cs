using SimpleDiscord.Networking;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleDiscord.Components
{
#pragma warning disable IDE1006
    public class Member : SocketMember
    {
        public Guild Guild { get; }

        public new List<Role> Roles { get; } = [];

        public VoiceState VoiceState
        {
            get
            {
                _voiceState ??= GetVoiceStateSync();
                return _voiceState;
            }
        }

        internal VoiceState _voiceState { get; set; } = null;

        public Member(Guild guild, SocketMember socketInstance, VoiceState voiceState = null) : base(socketInstance)
        {
            Guild = guild;
            _voiceState = voiceState;
            
            foreach (long role in socketInstance.Roles)
                Roles.Add(Guild.GetRole(role));
        }

        public Task AddRole(Role role, string reason = null) => AddRole(role.Id, reason);

        public Task AddRole(long role, string reason = null)
        {
            if (Roles.Any(r => r.Id == role))
                return Task.CompletedTask;

            return Guild.Client.RestHttp.MemberAddRole(this, role, reason);
        }

        public Task RemoveRole(Role role, string reason = null) => RemoveRole(role.Id, reason);

        public Task RemoveRole(long role, string reason = null)
        {
            if (!Roles.Any(r => r.Id == role))
                return Task.CompletedTask;

            return Guild.Client.RestHttp.MemberRemoveRole(this, role, reason);
        }

        public Task Kick(string reason = null) => Guild.Client.RestHttp.MemberKick(this, reason);

        public Task Ban(string reason = null, int deleteMessageSeconds = 0) => Guild.Client.RestHttp.MemberBan(this, reason, deleteMessageSeconds);

        public void SyncUser() => User = SocketUser.List.FirstOrDefault(u => u.Id == User.Id);

        public Task<VoiceState> GetVoiceState() => Guild.Client.RestHttp.GetMemberVoiceState(this);

        public VoiceState GetVoiceStateSync() => Http.Sync(GetVoiceState());

        public async Task Suppress()
        {
            if (!VoiceState.Connected)
                return;

            if (VoiceState.Suppress)
                await Guild.Client.RestHttp.UpdateMemberVoiceState(this, SocketSendVoiceState.Mute(true));
        }

        public async Task Unsuppress()
        {
            if (!VoiceState.Connected)
                return;

            if (VoiceState.Suppress)
                await Guild.Client.RestHttp.UpdateMemberVoiceState(this, SocketSendVoiceState.Mute(false));
        }

        public async Task Move(long channelId)
        {
            if (!VoiceState.Connected)
                return;

            if (VoiceState.Suppress)
                await Guild.Client.RestHttp.UpdateMemberVoiceState(this, SocketSendVoiceState.Move(channelId));
        }

        public async Task Move(GuildVoiceChannel channel) => await Move(channel.Id);

        public async Task Disconnect()
        {
            if (!VoiceState.Connected)
                return;

            if (VoiceState.Suppress)
                await Guild.Client.RestHttp.UpdateMemberVoiceState(this, new(null, null));
        }
    }
}
