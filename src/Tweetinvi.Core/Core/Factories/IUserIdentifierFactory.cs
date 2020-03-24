using Tweetinvi.Models;

namespace Tweetinvi.Core.Factories
{
    public interface IUserIdentifierFactory
    {
        IUserIdentifier Create(long userId);
        IUserIdentifier Create(string userScreenName);
    }
}