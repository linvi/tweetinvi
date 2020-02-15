namespace Tweetinvi.Parameters.Subscribers
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/post-lists-subscribers-create
    /// </summary>
    public interface IAddSubscriberToListParameters : ICustomRequestParameters
    {

    }

    public class AddSubscriberToListParameters : CustomRequestParameters, IAddSubscriberToListParameters
    {

    }
}