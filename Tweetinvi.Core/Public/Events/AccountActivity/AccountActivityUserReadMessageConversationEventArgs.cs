using Tweetinvi.Models;

namespace Tweetinvi.Events
{
    public enum UserReadMessageConversationInResultOf
    {
        /// <summary>
        /// Another user has read a message sent by the account user
        /// </summary>
        AnotherUserReadingMessageConversationWithAccountUser,

        /// <summary>
        /// This case should not happen and is here in case Twitter changes when they trigger the MessageRead event.
        /// If you happen to receive this mode, please report to Tweetinvi your case ideally with the associated json.
        /// </summary>
        Unknown
    }

    public class AccountActivityUserReadMessageConversationEventArgs : BaseAccountActivityEventArgs
    {
        public AccountActivityUserReadMessageConversationEventArgs(
            AccountActivityEvent activityEvent,
            IUser sender,
            IUser recipient,
            string lastReadEventId) 
            : base(activityEvent)
        {
            Sender = sender;
            Recipient = recipient;
            LastReadEventId = lastReadEventId;

            InResultOf = GetInResultOf();
        }

        public IUser Sender { get; }
        public IUser Recipient { get; }
        public string LastReadEventId { get; }

        public UserReadMessageConversationInResultOf InResultOf { get; }
        
        private UserReadMessageConversationInResultOf GetInResultOf()
        {
            if (Sender.Id != AccountUserId && Recipient.Id == AccountUserId)
            {
                return UserReadMessageConversationInResultOf.AnotherUserReadingMessageConversationWithAccountUser;
            }

            return UserReadMessageConversationInResultOf.Unknown;
        }
    }
}
