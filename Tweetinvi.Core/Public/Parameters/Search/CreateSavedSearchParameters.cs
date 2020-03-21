namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information read : https://developer.twitter.com/en/docs/accounts-and-users/manage-account-settings/api-reference/post-saved_searches-create
    /// </summary>
    public interface ICreateSavedSearchParameters : ICustomRequestParameters
    {
        /// <summary>
        /// The query of the search the user would like to save.
        /// </summary>
        string Query { get; set; }
    }

    /// <inheritdoc />
    public class CreateSavedSearchParameters : CustomRequestParameters, ICreateSavedSearchParameters
    {
        public CreateSavedSearchParameters(string query)
        {
            Query = query;
        }

        /// <inheritdoc />
        public string Query { get; set; }
    }
}