using Tweetinvi.Core.Factories;
using Tweetinvi.Models;

namespace Tweetinvi.Factories.Lists
{
    public class TwitterListIdentifierFactory : ITwitterListIdentifierFactory
    {
        public ITwitterListIdentifier Create(long listId)
        {
            return new TwitterListIdentifier(listId);
        }

        public ITwitterListIdentifier Create(string slug, IUserIdentifier user)
        {
            if (user == null)
            {
                return null;
            }

            if (user.Id != TweetinviSettings.DEFAULT_ID)
            {
                return Create(slug, user.Id);
            }

            if (!string.IsNullOrEmpty(user.ScreenName))
            {
                return Create(slug, user.ScreenName);
            }

            return null;
        }

        public ITwitterListIdentifier Create(string slug, long ownerId)
        {
            return new TwitterListIdentifier(slug, ownerId);
        }

        public ITwitterListIdentifier Create(string slug, string ownerScreenName)
        {
            return new TwitterListIdentifier(slug, ownerScreenName);
        }
    }
}