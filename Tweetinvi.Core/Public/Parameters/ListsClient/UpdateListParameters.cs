using Tweetinvi.Models;

namespace Tweetinvi.Parameters.ListsClient
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
            Id = parameters?.Id;
        }

        public UpdateListParameters(long? listId)
        {
            Id = new TwitterListIdentifier(listId);
        }

        public UpdateListParameters(string slug, IUserIdentifier userId)
        {
            Id = new TwitterListIdentifier(slug, userId);
        }

        public UpdateListParameters(ITwitterListIdentifier listId)
        {
            Id = listId;
        }

        public ITwitterListIdentifier Id { get; set; }
    }
}