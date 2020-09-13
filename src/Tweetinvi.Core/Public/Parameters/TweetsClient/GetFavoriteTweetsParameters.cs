using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://dev.twitter.com/en/docs/tweets/post-and-engage/api-reference/get-favorites-list
    /// </summary>
    public interface IGetUserFavoriteTweetsParameters : IMinMaxQueryParameters, ITweetModeParameter
    {
        /// <summary>
        /// The user from whom you want to get the favorite tweets
        /// </summary>
        IUserIdentifier User { get; set; }

        /// <summary>
        /// Include the tweet entities
        /// </summary>
        bool? IncludeEntities { get; set; }
    }

    /// <inheritdoc cref="IGetUserFavoriteTweetsParameters" />
    public class GetUserFavoriteTweetsParameters : MinMaxQueryParameters, IGetUserFavoriteTweetsParameters
    {
        public GetUserFavoriteTweetsParameters(string username) : this(new UserIdentifier(username))
        {
        }

        public GetUserFavoriteTweetsParameters(long userId) : this(new UserIdentifier(userId))
        {
        }

        public GetUserFavoriteTweetsParameters(IUserIdentifier user)
        {
            PageSize = TwitterLimits.DEFAULTS.TWEETS_GET_FAVORITE_TWEETS_MAX_SIZE;
            User = user;
        }

        public GetUserFavoriteTweetsParameters(IGetUserFavoriteTweetsParameters source) : base(source)
        {
            if (source == null)
            {
                PageSize = TwitterLimits.DEFAULTS.TWEETS_GET_FAVORITE_TWEETS_MAX_SIZE;
                return;
            }

            User = source.User;
            IncludeEntities = source.IncludeEntities;
        }

        /// <inheritdoc/>
        public IUserIdentifier User { get; set; }
        /// <inheritdoc/>
        public bool? IncludeEntities { get; set; }
        /// <inheritdoc/>
        public TweetMode? TweetMode { get; set; }
    }
}