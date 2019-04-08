using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Controllers
{
    public interface ITimelineController
    {
        // Home Timeline
        Task<IEnumerable<ITweet>> GetHomeTimeline(int maximumNumberOfTweetsToRetrieve);
        Task<IEnumerable<ITweet>> GetHomeTimeline(IHomeTimelineParameters timelineParameters);

        // User Timeline
        Task<IEnumerable<ITweet>> GetUserTimeline(IUserIdentifier user, int maximumNumberOfTweets = 40);
        Task<IEnumerable<ITweet>> GetUserTimeline(long userId, int maximumNumberOfTweets = 40);
        Task<IEnumerable<ITweet>> GetUserTimeline(string userScreenName, int maximumNumberOfTweets = 40);

        Task<IEnumerable<ITweet>> GetUserTimeline(long userId, IUserTimelineParameters parameters);
        Task<IEnumerable<ITweet>> GetUserTimeline(string userScreenName, IUserTimelineParameters parameters);
        Task<IEnumerable<ITweet>> GetUserTimeline(IUserIdentifier user, IUserTimelineParameters parameters);

        // Mention Timeline
        Task<IEnumerable<IMention>> GetMentionsTimeline(int maximumNumberOfTweets = 40);
        Task<IEnumerable<IMention>> GetMentionsTimeline(IMentionsTimelineParameters mentionsTimelineParameters);

        // Retweets Of Me Timeline
        Task<IEnumerable<ITweet>> GetRetweetsOfMeTimeline(IRetweetsOfMeTimelineParameters parameters);
    }
}