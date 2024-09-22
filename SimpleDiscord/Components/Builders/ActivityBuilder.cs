using SimpleDiscord.Enums;
using System.Linq;

namespace SimpleDiscord.Components.Builders
{
    public class ActivityBuilder(string name)
    {
        private readonly Activity activity = new(name, 0);

        public ActivityBuilder SetType(ActivityType type)
        {
            activity.Type = (int)type;
            return this;
        }

        public ActivityBuilder SetUrl(string url)
        {
            activity.Url = url;
            return this;
        }

        public ActivityBuilder SetDetails(string details)
        {
            activity.Details = details;
            return this;
        } 

        public ActivityBuilder SetState(string state)
        {
            activity.State = state;
            return this;
        }

        public ActivityBuilder SetEmoji(Emoji emoji)
        {
            activity.Emoji = emoji;
            return this;
        }

        public ActivityBuilder SetAssets(ActivityAsset assets)
        {
            activity.Assets = assets;
            return this;
        }

        public ActivityBuilder AddButton(ActivityButton button)
        {
            activity.Buttons ??= [];

            activity.Buttons.Append(button);
            return this;
        }

        public ActivityBuilder SetCreatedAt(long time)
        {
            activity.CreatedAt = time;
            return this;
        }

        public ActivityBuilder AddButton(string name, string url) => AddButton(new(name, url));

        public static ActivityBuilder New(string name) => new(name);

        public static implicit operator Activity(ActivityBuilder builder) => builder.activity;
    }
}
