namespace Tweetinvi.Parameters.ListsClient
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-list
    /// </summary>
    /// <inheritdoc />
    public interface IGetListsSubscribedByAccountParameters : ICustomRequestParameters
    {
        /// <summary>
        /// Set this to true if you would like owned lists to be returned first.
        /// </summary>
        bool? Reverse { get; set; }
    }

    /// <inheritdoc />
    public class GetListsSubscribedByAccountParameters : CustomRequestParameters, IGetListsSubscribedByAccountParameters
    {
        public GetListsSubscribedByAccountParameters()
        {
        }

        public GetListsSubscribedByAccountParameters(IGetListsSubscribedByAccountParameters parameters)
        {
            Reverse = parameters?.Reverse;
        }

        /// <inheritdoc />
        public bool? Reverse { get; set; }
    }
}