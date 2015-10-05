using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.Parameters.QueryParameters;

namespace Tweetinvi.Core.Interfaces.QueryGenerators
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