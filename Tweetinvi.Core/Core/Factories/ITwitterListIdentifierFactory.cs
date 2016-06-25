using Tweetinvi.Models;

namespace Tweetinvi.Core.Factories
{
    public interface ITwitterListIdentifierFactory
    {
        ITwitterListIdentifier Create(long listId);
        ITwitterListIdentifier Create(string slug, IUserIdentifier userIdentifier);
        ITwitterListIdentifier Create(string slug, long ownerId);
        ITwitterListIdentifier Create(string slug, string ownerScreenName);
    }
}