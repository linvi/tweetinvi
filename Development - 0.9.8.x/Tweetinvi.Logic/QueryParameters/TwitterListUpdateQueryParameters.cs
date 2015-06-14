using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Parameters;
using Tweetinvi.Core.Interfaces.Parameters.QueryParameters;

namespace Tweetinvi.Logic.QueryParameters
{
    public class TwitterListUpdateQueryParameters : ITwitterListUpdateQueryParameters
    {
        public TwitterListUpdateQueryParameters(ITwitterListIdentifier listIdentifier, ITwitterListUpdateParameters queryParameters)
        {
            TwitterListIdentifier = listIdentifier;
            QueryParameters = queryParameters;
        }

        public ITwitterListIdentifier TwitterListIdentifier { get; private set; }
        public ITwitterListUpdateParameters QueryParameters { get; private set; }
    }
}