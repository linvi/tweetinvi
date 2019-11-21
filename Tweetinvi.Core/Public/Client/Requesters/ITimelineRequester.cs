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
        /// Returns a collection of the most recent Tweets posted by the user indicated by the screen_name or user_id parameters.
        /// </summary>
        /// <para>Read more : https://developer.twitter.com/en/docs/tweets/timelines/api-reference/get-statuses-user_timeline </para>
        /// <returns>An iterator to list a user's timeline</returns>
        ITwitterPageIterator<ITwitterResult<ITweetDTO[]>, long?> GetUserTimelineIterator(IGetUserTimelineParameters parameters);
        
        /// <summary>
        /// Returns a collection of the most recent Tweets and Retweets posted by the authenticating user and the users they follow.
        /// The home timeline is central to how most users interact with the Twitter service.
        /// 
        /// Up to 800 Tweets are obtainable on the home timeline.
        /// It is more volatile for users that follow many users or follow users who Tweet frequently.
        /// </summary>
        /// <para>Read more : https://developer.twitter.com/en/docs/tweets/timelines/api-reference/get-statuses-home_timeline </para>
        /// <returns>An iterator to list the of tweets displayed on the authenticated user's home page</returns>
        ITwitterPageIterator<ITwitterResult<ITweetDTO[]>, long?> GetHomeTimelineIterator(IGetHomeTimelineParameters parameters);

        /// <summary>
        /// Returns the most recent Tweets authored by the authenticating user that have been retweeted by others.
        /// This timeline is a subset of the account user's timeline.
        /// <para>Read more : https://dev.twitter.com/en/docs/tweets/post-and-engage/api-reference/get-statuses-retweets_of_me </para> 
        /// </summary>
        /// <returns>An iterator to list the accounts tweet that got retweeted</returns>
        ITwitterPageIterator<ITwitterResult<ITweetDTO[]>, long?> GetRetweetsOfMeTimelineIterator(IGetRetweetsOfMeTimelineParameters parameters);

    }
}