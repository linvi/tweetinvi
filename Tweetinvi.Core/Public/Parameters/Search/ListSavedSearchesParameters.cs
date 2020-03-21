namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information read : https://developer.twitter.com/en/docs/accounts-and-users/manage-account-settings/api-reference/get-saved_searches-list
    /// </summary>
    public interface IListSavedSearchesParameters : ICustomRequestParameters
    {
    }

    public class ListSavedSearchesParameters : CustomRequestParameters, IListSavedSearchesParameters
    {
    }
}