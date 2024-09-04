namespace SimpleDiscord.Gateway.Events
{
    public interface IUserDeniableEvent
    {
        public bool CanShare { get; set; }
    }
}
