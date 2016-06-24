using Tweetinvi.Core.Parameters;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Logic.QueryParameters
{
    public class GetTweetsFromListQueryParameters : IGetTweetsFromListQueryParameters
    {
        public GetTweetsFromListQueryParameters(ITwitterListIdentifier listIdentifier, IGetTweetsFromListParameters parameters)
        {
            TwitterListIdentifier = listIdentifier;
            Parameters = parameters;
        }

        public ITwitterListIdentifier TwitterListIdentifier { get; private set; }
        public IGetTweetsFromListParameters Parameters { get; private set; }
    }
}