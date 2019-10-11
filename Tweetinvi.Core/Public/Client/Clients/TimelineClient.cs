using System.Threading.Tasks;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Iterators;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client
{
    public interface ITimelineClient
    {
        /// <summary>
        /// Validate all the TimelineClient parameters
        /// </summary>
        ITimelineClientParametersValidator ParametersValidator { get; }

        /// <inheritdoc cref="GetRetweetsOfMeTimeline(IGetRetweetsOfMeTimelineParameters)" />
        Task<ITweet[]> GetRetweetsOfMeTimeline();
        
        /// <summary>
        /// Returns the most recent Tweets authored by the authenticating user that have been retweeted by others.
        /// This timeline is a subset of the account user's timeline.
        /// <para>Read more : https://dev.twitter.com/en/docs/tweets/post-and-engage/api-reference/get-statuses-retweets_of_me </para> 
        /// </summary>
        /// <returns>The tweets retweeted by others</returns>
        Task<ITweet[]> GetRetweetsOfMeTimeline(IGetRetweetsOfMeTimelineParameters parameters);

        /// <inheritdoc cref="GetRetweetsOfMeTimelineIterator(IGetRetweetsOfMeTimelineParameters)" />
        ITwitterIterator<ITweet, long?> GetRetweetsOfMeTimelineIterator();
        
        /// <summary>
        /// Returns the most recent Tweets authored by the authenticating user that have been retweeted by others.
        /// This timeline is a subset of the account user's timeline.
        /// <para>Read more : https://dev.twitter.com/en/docs/tweets/post-and-engage/api-reference/get-statuses-retweets_of_me </para> 
        /// </summary>
        /// <returns>An iterator to list the the tweets that got retweeted by others</returns>
        ITwitterIterator<ITweet, long?> GetRetweetsOfMeTimelineIterator(IGetRetweetsOfMeTimelineParameters parameters);
    }
}