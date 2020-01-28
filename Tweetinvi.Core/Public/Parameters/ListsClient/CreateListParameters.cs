namespace Tweetinvi.Parameters.ListsClient
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/post-lists-create
    /// </summary>
    /// <inheritdoc />
    public interface ICreateListParameters : IListMetadataParameters
    {
    }

    /// <inheritdoc />
    public class CreateListParameters : ListMetadataParameters, ICreateListParameters
    {
        public CreateListParameters(string name)
        {
            Name = name;
        }

        public CreateListParameters(IListMetadataParameters parameters) : base(parameters)
        {
        }
    }
}