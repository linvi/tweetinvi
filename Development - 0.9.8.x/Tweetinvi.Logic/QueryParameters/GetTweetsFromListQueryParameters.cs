using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Parameters;
using Tweetinvi.Logic.Parameters.QueryParameters;

namespace Tweetinvi.Logic.QueryParameters
{
    public class GetTweetsFromListQueryParameters : IGetTweetsFromListQueryParameters
    {
        public GetTweetsFromListQueryParameters(ITwitterListIdentifier listIdentifier, IGetTweetsFromListParameters queryParameters)
        {
            TwitterListIdentifier = listIdentifier;
            QueryParameters = queryParameters;
        }

        public ITwitterListIdentifier TwitterListIdentifier { get; private set; }
        public IGetTweetsFromListParameters QueryParameters { get; private set; }
    }
}