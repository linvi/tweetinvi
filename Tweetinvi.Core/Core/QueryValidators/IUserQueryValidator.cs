using Tweetinvi.Models;

namespace Tweetinvi.Core.QueryValidators
{
    public interface IUserQueryValidator
    {
        bool CanUserBeIdentified(IUserIdentifier userIdentifier);
        bool IsScreenNameValid(string screenName);
        bool IsUserIdValid(long? userId);

        void ThrowIfUserCannotBeIdentified(IUserIdentifier userIdentifier);
        void ThrowIfUserCannotBeIdentified(IUserIdentifier userIdentifier, string parameterName);
    }
}