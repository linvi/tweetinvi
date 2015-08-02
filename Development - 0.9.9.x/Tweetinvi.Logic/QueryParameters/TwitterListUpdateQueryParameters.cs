using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Parameters;
using Tweetinvi.Core.Interfaces.Parameters.QueryParameters;

namespace Tweetinvi.Logic.QueryParameters
{
    public class TwitterListUpdateQueryParameters : ITwitterListUpdateQueryParameters
    {
        public TwitterListUpdateQueryParameters(ITwitterListIdentifier listIdentifier, ITwitterListUpdateParameters parameters)
        {
            TwitterListIdentifier = listIdentifier;
            Parameters = parameters;
        }

        public ITwitterListIdentifier TwitterListIdentifier { get; private set; }
        public ITwitterListUpdateParameters Parameters { get; private set; }
    }
}