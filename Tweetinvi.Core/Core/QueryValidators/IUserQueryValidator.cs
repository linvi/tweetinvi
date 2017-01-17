using Tweetinvi.Models;

namespace Tweetinvi.Core.QueryValidators
{
    public interface IUserQueryValidator
    {
        bool CanUserBeIdentified(IUserIdentifier user);
        bool IsScreenNameValid(string screenName);
        bool IsUserIdValid(long? userId);

        void ThrowIfUserCannotBeIdentified(IUserIdentifier user);
        void ThrowIfUserCannotBeIdentified(IUserIdentifier user, string parameterName);
    }
}