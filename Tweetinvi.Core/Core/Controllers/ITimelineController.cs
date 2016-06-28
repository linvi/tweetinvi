using System.Collections.Generic;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Controllers
{
    public interface ITimelineController
    {
        // Home Timeline
        IEnumerable<ITweet> GetHomeTimeline(int maximumNumberOfTweetsToRetrieve);
        IEnumerable<ITweet> GetHomeTimeline(IHomeTimelineParameters timelineParameters);

        // User Timeline
        IEnumerable<ITweet> GetUserTimeline(IUserIdentifier userIdentifier, int maximumNumberOfTweets = 40);
        IEnumerable<ITweet> GetUserTimeline(long userId, int maximumNumberOfTweets = 40);
        IEnumerable<ITweet> GetUserTimeline(string userScreenName, int maximumNumberOfTweets = 40);

        IEnumerable<ITweet> GetUserTimeline(long userId, IUserTimelineParameters parameters);
        IEnumerable<ITweet> GetUserTimeline(string userScreenName, IUserTimelineParameters parameters);
        IEnumerable<ITweet> GetUserTimeline(IUserIdentifier user, IUserTimelineParameters parameters);

        // Mention Timeline
        IEnumerable<IMention> GetMentionsTimeline(int maximumNumberOfTweets = 40);
        IEnumerable<IMention> GetMentionsTimeline(IMentionsTimelineParameters mentionsTimelineParameters);

        // Retweets Of Me Timeline
        IEnumerable<ITweet> GetRetweetsOfMeTimeline(IRetweetsOfMeTimelineParameters parameters);
    }
}