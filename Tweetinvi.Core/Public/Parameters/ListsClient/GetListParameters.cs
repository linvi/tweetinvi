using Tweetinvi.Models;

namespace Tweetinvi.Parameters
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
        public GetListParameters(long listId) : this(new TwitterListIdentifier(listId))
        {
        }

        public GetListParameters(string slug, IUserIdentifier userId) : this(new TwitterListIdentifier(slug, userId))
        {
        }

        public GetListParameters(ITwitterListIdentifier list) : base(list)
        {
        }
    }
}