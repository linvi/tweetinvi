namespace Tweetinvi.Models
{
    public class TwitterListIdentifier : ITwitterListIdentifier
    {
        private TwitterListIdentifier()
        {
            Id = TweetinviSettings.DEFAULT_ID;
            Owner = new UserIdentifier(TweetinviSettings.DEFAULT_ID);
        }

        public TwitterListIdentifier(long listId) : this()
        {
            Id = listId;
        }

        public TwitterListIdentifier(string slug, long ownerId) : this()
        {
            Slug = slug;
            Owner = new UserIdentifier(ownerId);
        }

        public TwitterListIdentifier(string slug, string ownerScreenName) : this()
        {
            Slug = slug;
            Owner = new UserIdentifier(ownerScreenName);
        }

        public TwitterListIdentifier(string slug, IUserIdentifier owner)
        {
            Slug = slug;
            Owner = owner;
        }

        public long Id { get; private set; }
        public string Slug { get; private set; }

        public long OwnerId
        {
            get { return Owner.Id; }
        }

        public string OwnerScreenName
        {
            get { return Owner.ScreenName; }
        }

        public IUserIdentifier Owner { get; private set; }
    }
}