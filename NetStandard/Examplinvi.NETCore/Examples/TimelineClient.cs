using System.Collections.Generic;
using Tweetinvi;
using Tweetinvi.Models;

namespace Examplinvi.NETCore.Examples
{
    interface ITimelineClient
    {
        IEnumerable<ITweet> GetHomeTimeline();
        IEnumerable<IMention> GetMentionsTimeline();
        IEnumerable<ITweet> GetUserTimeline(string username);
    }

    public class TimelineClient : ITimelineClient
    {
        public IEnumerable<ITweet> GetHomeTimeline()
        {
            var user = User.GetAuthenticatedUser();
            return user.GetHomeTimeline();
        }

        public IEnumerable<IMention> GetMentionsTimeline()
        {
            var user = User.GetAuthenticatedUser();
            return user.GetMentionsTimeline();
        }

        public IEnumerable<ITweet> GetUserTimeline(string username)
        {
            var user = User.GetUserFromScreenName(username);
            return user.GetUserTimeline();
        }
    }
}
