using Tweetinvi.Core.Extensions;
using Tweetinvi.Models;
using Tweetinvi.Parameters.Optionals;

namespace Tweetinvi.Parameters
{
    public interface IGetFavoriteTweetsParameters : IMaxAndMinBaseQueryParameters, IGetUsersOptionalParameters
    {
        IUserIdentifier UserIdentifier { get; set; }
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
            SkipStatus = source.SkipStatus;
            IncludeEntities = source.IncludeEntities;
        }

        public IUserIdentifier UserIdentifier { get; set; }
        public bool? SkipStatus { get; set; }
        public bool? IncludeEntities { get; set; }
    }
}