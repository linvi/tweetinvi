using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client.Requesters
{
    /// <summary>
    /// A client providing all the actions relative to timelines.
    /// The results from this client contain additional metadata.
    /// </summary>
    public interface ITimelineRequester
    {
        /// <summary>
        /// Returns the most recent Tweets authored by the authenticating user that have been retweeted by others.
        /// This timeline is a subset of the account user's timeline.
        /// <para>Read more : https://dev.twitter.com/en/docs/tweets/post-and-engage/api-reference/get-statuses-retweets_of_me </para> 
        /// </summary>
        /// <returns>An iterator to list the the accounts tweet that got retweeted</returns>
        ITwitterPageIterator<ITwitterResult<ITweetDTO[]>, long?> GetRetweetsOfMeTimeline(IGetRetweetsOfMeTimelineParameters parameters);
    }
}