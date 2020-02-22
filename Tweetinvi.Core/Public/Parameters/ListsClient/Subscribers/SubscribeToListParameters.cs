using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/post-lists-subscribers-create
    /// </summary>
    public interface ISubscribeToListParameters : IListParameters
    {
    }

    public class SubscribeToListParameters : ListParameters, ISubscribeToListParameters
    {
        public SubscribeToListParameters(long? listId) : base(listId)
        {
        }

        public SubscribeToListParameters(ITwitterListIdentifier list) : base(list)
        {
        }
    }
}