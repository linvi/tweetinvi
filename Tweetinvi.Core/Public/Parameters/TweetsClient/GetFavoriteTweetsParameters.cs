using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/tweets/post-and-engage/api-reference/get-favorites-list
    /// </summary>
    public interface IGetFavoriteTweetsParameters : IMaxAndMinBaseQueryParameters
    {
        /// <summary>
        /// The user from whom you want to get his favorite tweets
        /// </summary>
        IUserIdentifier UserIdentifier { get; set; }
        
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
            PageSize = 200;
            UserIdentifier = user;
        }

        public GetFavoriteTweetsParameters(IGetFavoriteTweetsParameters source) : base(source)
        {
            if (source == null)
            {
                return;
            }

            UserIdentifier = source.UserIdentifier;
            IncludeEntities = source.IncludeEntities;
        }

        /// <inheritdoc/>
        public IUserIdentifier UserIdentifier { get; set; }
        
        /// <inheritdoc/>
        public bool? IncludeEntities { get; set; }
    }
}