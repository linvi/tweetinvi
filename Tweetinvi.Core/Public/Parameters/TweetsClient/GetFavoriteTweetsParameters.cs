using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://dev.twitter.com/en/docs/tweets/post-and-engage/api-reference/get-favorites-list
    /// </summary>
    public interface IGetFavoriteTweetsParameters : IMaxAndMinBaseQueryParameters
    {
        /// <summary>
        /// The user from whom you want to get his favorite tweets
        /// </summary>
        IUserIdentifier User { get; set; }
        
        /// <summary>
        /// Include the tweet entities
        /// </summary>
        bool? IncludeEntities { get; set; }
    }

    /// <inheritdoc cref="IGetFavoriteTweetsParameters" />
    public class GetFavoriteTweetsParameters : MaxAndMinBaseQueryParameters, IGetFavoriteTweetsParameters
    {
        public GetFavoriteTweetsParameters(string username) : this(new UserIdentifier(username))
        {
        }

        public GetFavoriteTweetsParameters(long? userId) : this(new UserIdentifier(userId))
        {
        }

        public GetFavoriteTweetsParameters(IUserIdentifier user)
        {
            PageSize = TwitterLimits.DEFAULTS.TWEETS_GET_FAVORITE_TWEETS_MAX_SIZE;
            User = user;
        }

        public GetFavoriteTweetsParameters(IGetFavoriteTweetsParameters source) : base(source)
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
    }
}