namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information read : https://developer.twitter.com/en/docs/accounts-and-users/manage-account-settings/api-reference/get-saved_searches-show-id
    /// </summary>
    public interface IGetSavedSearchParameters : ICustomRequestParameters
    {
        /// <summary>
        /// The ID of the saved search.
        /// </summary>
        long SavedSearchId { get; set; }
    }

    /// <inheritdoc />
    public class GetSavedSearchParameters : CustomRequestParameters, IGetSavedSearchParameters
    {
        public GetSavedSearchParameters(long savedSearchId)
        {
            SavedSearchId = savedSearchId;
        }

        /// <inheritdoc />
        public long SavedSearchId { get; set; }
    }
}