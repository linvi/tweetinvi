using Tweetinvi.Models;

namespace Tweetinvi.Parameters.Subscribers
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-subscribers-show
    /// </summary>
    public interface ICheckIfUserIsSubscriberOfListParameters : IListParameters
    {

    }


    public class CheckIfUserIsSubscriberOfListParameters : ListParameters, ICheckIfUserIsSubscriberOfListParameters
    {
        public CheckIfUserIsSubscriberOfListParameters(ITwitterListIdentifier list) : base(list)
        {
        }
    }
}