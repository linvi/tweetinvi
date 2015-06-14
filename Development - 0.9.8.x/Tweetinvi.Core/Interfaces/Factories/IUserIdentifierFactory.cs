using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Interfaces.Factories
{
    public interface IUserIdentifierFactory
    {
        IUserIdentifier Create(long userId);
        IUserIdentifier Create(string userScreenName);
    }
}