using Tweetinvi.Core.Parameters;
using Tweetinvi.Models;

namespace Tweetinvi.Parameters.Subscribers
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-subscribers
    /// </summary>
    public interface IGetSubscribersOfList : IBaseGetUsersOfListParameters
    {
    }

    /// <inheritdoc />
    public class GetSubscribersOfList : BaseGetUsersOfListParameters, IGetSubscribersOfList
    {
        public GetSubscribersOfList(long? listId) : this(new TwitterListIdentifier(listId))
        {
        }

        public GetSubscribersOfList(ITwitterListIdentifier list) : base(list)
        {
            PageSize = TwitterLimits.DEFAULTS.LISTS_GET_SUBSCRIBERS_MAX_SIZE;
        }

        public GetSubscribersOfList(IGetSubscribersOfList parameters) : base(parameters)
        {
            if (parameters == null)
            {
                PageSize = TwitterLimits.DEFAULTS.LISTS_GET_SUBSCRIBERS_MAX_SIZE;
            }
        }
    }
}