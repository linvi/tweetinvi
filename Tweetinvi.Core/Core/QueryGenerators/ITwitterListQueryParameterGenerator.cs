using Tweetinvi.Core.Parameters;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.QueryGenerators
{
    public interface ITwitterListQueryParameterGenerator
    {
        string GenerateIdentifierParameter(ITwitterListIdentifier twitterListIdentifier);
        
        // User Parameters
        IGetTweetsFromListParameters CreateTweetsFromListParameters();
        ITwitterListUpdateParameters CreateUpdateListParameters();

        // Query Parameters
        IGetTweetsFromListQueryParameters CreateTweetsFromListQueryParameters(
            ITwitterListIdentifier listIdentifier,
            IGetTweetsFromListParameters getTweetsFromListParameters);

        ITwitterListUpdateQueryParameters CreateTwitterListUpdateQueryParameters(
            ITwitterListIdentifier listIdentifier,
            ITwitterListUpdateParameters listUpdateParameters);
    }
}