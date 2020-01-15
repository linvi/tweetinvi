using System.Threading.Tasks;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Iterators;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client
{
    public interface ITimelinesClient
    {
        /// <summary>
        /// Validate all the TimelineClient parameters
        /// </summary>
        ITimelineClientParametersValidator ParametersValidator { get; }

        /// <inheritdoc cref="GetHomeTimelineIterator(IGetHomeTimelineParameters)" />
        ITwitterIterator<ITweet, long?> GetHomeTimelineIterator();

        /// <summary>
        /// Returns a collection of the most recent Tweets and Retweets posted by the authenticated user and the users they follow.
        /// The home timeline is central to how most users interact with the Twitter service.
        ///
        /// Up to 800 Tweets are obtainable on the home timeline.
        /// It is more volatile for users that follow many users or follow users who Tweet frequently.
        /// </summary>
        /// <para>Read more : https://developer.twitter.com/en/docs/tweets/timelines/api-reference/get-statuses-home_timeline </para>
        /// <returns>An iterator to list the of tweets displayed on the authenticated user's home page</returns>
        ITwitterIterator<ITweet, long?> GetHomeTimelineIterator(IGetHomeTimelineParameters parameters);


        /// <inheritdoc cref="GetMentionsTimelineIterator(IGetMentionsTimelineParameters)" />
        ITwitterIterator<ITweet, long?> GetMentionsTimelineIterator();

        /// <summary>
        /// Returns the most recent mentions (Tweets containing a users's @screen_name) for the authenticated user.
        /// The timeline returned is the equivalent of the one seen when you view your mentions on twitter.com.
        /// </summary>
        /// <para>Read more : https://developer.twitter.com/en/docs/tweets/timelines/api-reference/get-statuses-mentions_timeline </para>
        /// <returns>An iterator to list the of tweets mentioning the authenticated user's</returns>
        ITwitterIterator<ITweet, long?> GetMentionsTimelineIterator(IGetMentionsTimelineParameters parameters);


        /// <inheritdoc cref="GetUserTimelineIterator(IGetUserTimelineParameters)" />
        ITwitterIterator<ITweet, long?> GetUserTimelineIterator(long? userId);

        /// <inheritdoc cref="GetUserTimelineIterator(IGetUserTimelineParameters)" />
        ITwitterIterator<ITweet, long?> GetUserTimelineIterator(string username);

        /// <inheritdoc cref="GetUserTimelineIterator(IGetUserTimelineParameters)" />
        ITwitterIterator<ITweet, long?> GetUserTimelineIterator(IUserIdentifier user);

        /// <summary>
        /// Returns a collection of the most recent Tweets posted by the user indicated by the screen_name or user_id parameters.
        /// </summary>
        /// <para>Read more : https://developer.twitter.com/en/docs/tweets/timelines/api-reference/get-statuses-user_timeline </para>
        /// <returns>An iterator to list a user's timeline</returns>
        ITwitterIterator<ITweet, long?> GetUserTimelineIterator(IGetUserTimelineParameters parameters);



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
        /// <returns>An iterator to list the tweets that got retweeted by others</returns>
        ITwitterIterator<ITweet, long?> GetRetweetsOfMeTimelineIterator(IGetRetweetsOfMeTimelineParameters parameters);
    }
}