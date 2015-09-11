namespace Tweetinvi.Core.Interfaces.Models
{
    public interface ITwitterListIdentifier
    {
        long Id { get; }
        string Slug { get; }

        long OwnerId { get; }
        string OwnerScreenName { get; }
    }
}