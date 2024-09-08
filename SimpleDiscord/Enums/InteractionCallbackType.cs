namespace SimpleDiscord.Enums
{
    public enum InteractionCallbackType
    {
        PONG = 1,
        CHANNEL_MESSAGE_WITH_SOURCE = 4,
        DEFERRED_CHANNEL_MESSAGE_WITH_SOURCE,
        DEFERRED_UPDATE_MESSAGE,
        UPDATE_MESSAGE,
        APPLICATION_COMMAND_AUTOCOMPLETE_RESULT,
        MODAL,
        PREMIUM_REQUIRED,
        LAUNCH_ACTIVITY
    }
}
