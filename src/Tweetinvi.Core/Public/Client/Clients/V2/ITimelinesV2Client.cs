using Tweetinvi.Iterators;
using Tweetinvi.Models.V2;
using Tweetinvi.Parameters.V2;

namespace Tweetinvi.Client.V2
{
    public interface ITimelinesV2Client
    {
        /// <summary>
        /// Returns Tweets composed by a single user, specified by the requested user ID. By default,
        /// the most recent ten Tweets are returned per request. Using pagination, the most recent
        /// 3,200 Tweets can be retrieved.
        /// <para>Read more : https://developer.twitter.com/en/docs/twitter-api/tweets/timelines/api-reference/get-users-id-tweets </para>
        /// </summary>
        /// <returns>The Timeline</returns>
        ITwitterRequestIterator<TimelinesV2Response, string> GetUserTweetsTimelineIterator(IGetTimelinesV2Parameters parameters);

        /// <summary>
        /// Returns Tweets mentioning a single user specified by the requested user ID. By default,
        /// the most recent ten Tweets are returned per request. Using pagination, up to the most 
        /// recent 800 Tweets can be retrieved.
        /// <para>Read more : https://developer.twitter.com/en/docs/twitter-api/tweets/timelines/api-reference/get-users-id-mentions </para>
        /// </summary>
        ITwitterRequestIterator<TimelinesV2Response, string> GetUserMentionedTimelineIterator(IGetTimelinesV2Parameters parameters);

    }
}
