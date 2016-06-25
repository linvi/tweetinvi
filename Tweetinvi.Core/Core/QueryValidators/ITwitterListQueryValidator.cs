using Tweetinvi.Core.Parameters;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.QueryValidators
{
    public interface ITwitterListQueryValidator
    {
        // Identifiers
        bool IsListIdentifierValid(ITwitterListIdentifier parameters);

        // Throw
        void ThrowIfListIdentifierIsNotValid(ITwitterListIdentifier twitterListIdentifier);
        void ThrowIfListUpdateParametersIsNotValid(ITwitterListUpdateParameters parameters);
        void ThrowIfGetTweetsFromListQueryParametersIsNotValid(IGetTweetsFromListQueryParameters parameters);
    }
}