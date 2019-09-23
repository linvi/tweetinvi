using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    public interface IGetFavoriteTweetsParameters : IMaxAndMinBaseQueryParameters
    {
        IUserIdentifier UserIdentifier { get; set; }
        bool? IncludeEntities { get; set; }
    }

    /// <summary>
    /// https://developer.twitter.com/en/docs/tweets/post-and-engage/api-reference/get-favorites-list
    /// </summary>
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

        public IUserIdentifier UserIdentifier { get; set; }
        public bool? IncludeEntities { get; set; }
    }
}