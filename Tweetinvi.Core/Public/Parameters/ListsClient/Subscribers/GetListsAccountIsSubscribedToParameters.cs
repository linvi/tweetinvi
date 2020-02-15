namespace Tweetinvi.Parameters.Subscribers
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-subscriptions
    /// </summary>
    public interface IGetListsAccountIsMemberOfParameters : ICursorQueryParameters
    {

    }

    public class GetListsAccountIsMemberOfParameters : CursorQueryParameters, IGetListsAccountIsMemberOfParameters
    {

    }
}