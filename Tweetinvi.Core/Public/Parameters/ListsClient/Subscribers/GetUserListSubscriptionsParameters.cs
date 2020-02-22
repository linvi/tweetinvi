using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-subscriptions
    /// </summary>
    public interface IGetUserListSubscriptionsParameters : IGetAccountListSubscriptionsParameters
    {
        /// <summary>
        /// User from whom you want the lists he is subscribed to
        /// </summary>
        IUserIdentifier User { get; set; }
    }

    public class GetUserListSubscriptionsParameters : CursorQueryParameters, IGetUserListSubscriptionsParameters
    {
        public GetUserListSubscriptionsParameters(long? userId) : this(new UserIdentifier(userId))
        {
        }

        public GetUserListSubscriptionsParameters(string username) : this(new UserIdentifier(username))
        {
        }

        public GetUserListSubscriptionsParameters(IUserIdentifier user)
        {
            User = user;
        }

        public GetUserListSubscriptionsParameters(IGetUserListSubscriptionsParameters parameters) : base(parameters)
        {
            User = parameters?.User;
        }

        public GetUserListSubscriptionsParameters(IGetAccountListSubscriptionsParameters parameters) : base(parameters)
        {
        }

        /// <inheritdoc />
        public IUserIdentifier User { get; set; }
    }
}