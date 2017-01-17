using Tweetinvi.Core.Parameters;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Logic.QueryParameters
{
    public class UserTimelineQueryParameters : IUserTimelineQueryParameters
    {
        public UserTimelineQueryParameters(
            IUserIdentifier user,
            IUserTimelineParameters parameters)
        {
            UserIdentifier = user;
            Parameters = parameters;
        }

        public IUserIdentifier UserIdentifier { get; private set; }
        public IUserTimelineParameters Parameters { get; private set; }
    }
}