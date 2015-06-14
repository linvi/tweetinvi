using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Parameters;
using Tweetinvi.Logic.Parameters.QueryParameters;

namespace Tweetinvi.Logic.QueryParameters
{
    public class UserTimelineQueryParameters : IUserTimelineQueryParameters
    {
        public UserTimelineQueryParameters(
            IUserIdentifier userIdentifier,
            IUserTimelineParameters queryParameters)
        {
            UserIdentifier = userIdentifier;
            QueryParameters = queryParameters;
        }

        public IUserIdentifier UserIdentifier { get; private set; }
        public IUserTimelineParameters QueryParameters { get; private set; }
    }
}