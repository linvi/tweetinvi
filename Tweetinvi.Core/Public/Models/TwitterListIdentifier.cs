namespace Tweetinvi.Models
{
    public class TwitterListIdentifier : ITwitterListIdentifier
    {
        private TwitterListIdentifier()
        {
            Owner = new UserIdentifier();
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

        public long Id { get; }
        public string Slug { get; }

        public long OwnerId => Owner?.Id ?? 0;
        public string OwnerScreenName => Owner?.ScreenName;

        public IUserIdentifier Owner { get; }
    }
}