using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.Parameters.QueryParameters;

namespace Tweetinvi.Core.Interfaces.QueryValidators
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