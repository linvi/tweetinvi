using Tweetinvi.Models;

namespace Tweetinvi.Core.QueryValidators
{
    public interface IUserQueryValidator
    {
        void ThrowIfUserCannotBeIdentified(IUserIdentifier user);
        void ThrowIfUserCannotBeIdentified(IUserIdentifier user, string parameterName);
    }
}