using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.Parameters.QueryParameters;

namespace Tweetinvi.Logic.QueryParameters
{
    public class UserTimelineQueryParameters : IUserTimelineQueryParameters
    {
        public UserTimelineQueryParameters(
            IUserIdentifier userIdentifier,
            IUserTimelineParameters parameters)
        {
            UserIdentifier = userIdentifier;
            Parameters = parameters;
        }

        public IUserIdentifier UserIdentifier { get; private set; }
        public IUserTimelineParameters Parameters { get; private set; }
    }
}