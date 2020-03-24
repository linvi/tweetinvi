using Tweetinvi.Parameters.Optionals;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-friendships-incoming
    /// </summary>
    /// <inheritdoc />
    public interface IGetUsersYouRequestedToFollowParameters : IGetCursorUsersOptionalParameters, IGetUserIdsYouRequestedToFollowParameters
    {
        /// <summary>
        /// Page size when retrieving the users objects from Twitter
        /// </summary>
        int GetUsersPageSize { get; set; }
    }

    /// <inheritdoc />
    public class GetUsersYouRequestedToFollowParameters : GetCursorUsersOptionalParameters, IGetUsersYouRequestedToFollowParameters
    {
        public GetUsersYouRequestedToFollowParameters()
        {
            GetUsersPageSize = TwitterLimits.DEFAULTS.USERS_GET_USERS_MAX_SIZE;
        }

        public GetUsersYouRequestedToFollowParameters(IGetUsersYouRequestedToFollowParameters source) : base(source)
        {
            if (source == null)
            {
                GetUsersPageSize = TwitterLimits.DEFAULTS.USERS_GET_USERS_MAX_SIZE;
                return;
            }

            GetUsersPageSize = source.GetUsersPageSize;
        }

        /// <inheritdoc/>
        public int GetUsersPageSize { get; set; }
    }
}