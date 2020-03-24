using Tweetinvi.Models;

namespace Tweetinvi.Core.QueryValidators
{
    public interface ITwitterListQueryValidator
    {
        // Identifiers
        bool IsListIdentifierValid(ITwitterListIdentifier parameters);

        // Throw
        void ThrowIfListIdentifierIsNotValid(ITwitterListIdentifier twitterListIdentifier);
    }
}