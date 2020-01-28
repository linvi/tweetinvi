using Tweetinvi.Models;

namespace Tweetinvi.Parameters.ListsClient
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-show
    /// </summary>
    /// <inheritdoc />
    public interface IGetListParameters : IListParameters
    {

    }

    public class GetListParameters : ListParameters, IGetListParameters
    {
        public GetListParameters(long? listId) : this(new TwitterListIdentifier(listId))
        {
        }

        public GetListParameters(ITwitterListIdentifier listId) : base(listId)
        {
        }
    }
}