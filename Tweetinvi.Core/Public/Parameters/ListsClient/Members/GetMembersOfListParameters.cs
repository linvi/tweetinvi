using Tweetinvi.Core.Parameters;
using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-members
    /// </summary>
    public interface IGetMembersOfListParameters : IBaseGetUsersOfListParameters
    {
    }

    /// <inheritdoc />
    public class GetMembersOfListParameters : BaseGetUsersOfListParameters, IGetMembersOfListParameters
    {
        public GetMembersOfListParameters(long? listId) : this(new TwitterListIdentifier(listId))
        {
        }

        public GetMembersOfListParameters(ITwitterListIdentifier list) : base(list)
        {
            PageSize = TwitterLimits.DEFAULTS.LISTS_GET_MEMBERS_MAX_PAGE_SIZE;
        }

        public GetMembersOfListParameters(IGetMembersOfListParameters parameters) : base(parameters)
        {
            if (parameters == null)
            {
                PageSize = TwitterLimits.DEFAULTS.LISTS_GET_MEMBERS_MAX_PAGE_SIZE;
            }
        }
    }
}