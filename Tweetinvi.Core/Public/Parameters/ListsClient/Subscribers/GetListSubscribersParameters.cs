using Tweetinvi.Core.Parameters;
using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-subscribers
    /// </summary>
    public interface IGetListSubscribersParameters : IBaseGetUsersOfListParameters
    {
    }

    /// <inheritdoc />
    public class GetListSubscribersParameters : BaseGetUsersOfListParameters, IGetListSubscribersParameters
    {
        public GetListSubscribersParameters(long listId) : this(new TwitterListIdentifier(listId))
        {
        }

        public GetListSubscribersParameters(ITwitterListIdentifier list) : base(list)
        {
            PageSize = TwitterLimits.DEFAULTS.LISTS_GET_SUBSCRIBERS_MAX_PAGE_SIZE;
        }

        public GetListSubscribersParameters(IGetListSubscribersParameters parameters) : base(parameters)
        {
            if (parameters == null)
            {
                PageSize = TwitterLimits.DEFAULTS.LISTS_GET_SUBSCRIBERS_MAX_PAGE_SIZE;
            }
        }
    }
}