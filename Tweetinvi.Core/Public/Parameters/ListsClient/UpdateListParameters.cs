using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/post-lists-update
    /// </summary>
    /// <inheritdoc />
    public interface IUpdateListParameters : IListMetadataParameters, IListParameters
    {
    }

    /// <inheritdoc />
    public class UpdateListParameters : ListMetadataParameters, IUpdateListParameters
    {
        public UpdateListParameters(IUpdateListParameters parameters) : base(parameters)
        {
            List = parameters?.List;
        }

        public UpdateListParameters(long? listId)
        {
            List = new TwitterListIdentifier(listId);
        }

        public UpdateListParameters(string slug, IUserIdentifier userId)
        {
            List = new TwitterListIdentifier(slug, userId);
        }

        public UpdateListParameters(ITwitterListIdentifier listId)
        {
            List = listId;
        }

        public ITwitterListIdentifier List { get; set; }
    }
}