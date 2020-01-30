using Tweetinvi.Models;

namespace Tweetinvi.Parameters.ListsClient
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-members
    /// </summary>
    /// <inheritdoc />
    public interface IGetMembersOfListParameters : IListParameters, ICursorQueryParameters
    {
    }

    /// <inheritdoc />
    public class GetMembersOfListParameters : CursorQueryParameters, IGetMembersOfListParameters
    {
        public GetMembersOfListParameters(long? listId) : this(new TwitterListIdentifier(listId))
        {
        }

        public GetMembersOfListParameters(ITwitterListIdentifier list)
        {
            List = list;
            PageSize = TwitterLimits.DEFAULTS.LISTS_GET_MEMBERS_MAX_SIZE;
        }

        public GetMembersOfListParameters(IGetMembersOfListParameters parameters) : base(parameters)
        {
            if (parameters == null)
            {
                PageSize = TwitterLimits.DEFAULTS.LISTS_GET_MEMBERS_MAX_SIZE;
            }

            List = parameters?.List;
        }

        /// <inheritdoc />
        public ITwitterListIdentifier List { get; set; }
    }
}