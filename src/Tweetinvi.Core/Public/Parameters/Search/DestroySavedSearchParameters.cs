using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information read : https://developer.twitter.com/en/docs/accounts-and-users/manage-account-settings/api-reference/post-saved_searches-destroy-id
    /// </summary>
    public interface IDestroySavedSearchParameters : ICustomRequestParameters
    {
        /// <summary>
        /// The ID of the saved search.
        /// </summary>
        long SavedSearchId { get; set; }
    }

    /// <inheritdoc />
    public class DestroySavedSearchParameters : CustomRequestParameters, IDestroySavedSearchParameters
    {
        public DestroySavedSearchParameters(long savedSearchId)
        {
            SavedSearchId = savedSearchId;
        }

        public DestroySavedSearchParameters(ISavedSearch savedSearch)
        {
            SavedSearchId = savedSearch.Id;
        }

        /// <inheritdoc />
        public long SavedSearchId { get; set; }
    }
}