namespace Tweetinvi.Parameters.Subscribers
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/post-lists-subscribers-destroy
    /// </summary>
    public interface IUnsubscribeFromListParameters : ICursorQueryParameters
    {

    }

    public class UnsubscribeFromListParameters : CursorQueryParameters, IUnsubscribeFromListParameters
    {

    }
}