using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Parameters
{
    public class TwitterListIdentifier : ITwitterListIdentifier
    {
        private TwitterListIdentifier()
        {
            Id = TweetinviSettings.DEFAULT_ID;
            OwnerId = TweetinviSettings.DEFAULT_ID;
        }

        public TwitterListIdentifier(long listId) : this()
        {
            Id = listId;
        }

        public TwitterListIdentifier(string slug, long ownerId) : this()
        {
            Slug = slug;
            OwnerId = ownerId;
        }

        public TwitterListIdentifier(string slug, string ownerScreenName) : this()
        {
            Slug = slug;
            OwnerScreenName = ownerScreenName;
        }

        public long Id { get; private set; }
        public string Slug { get; private set; }
        public long OwnerId { get; private set; }
        public string OwnerScreenName { get; private set; }
    }
}