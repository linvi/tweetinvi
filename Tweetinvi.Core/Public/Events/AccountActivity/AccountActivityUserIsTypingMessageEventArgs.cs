using Tweetinvi.Models;

namespace Tweetinvi.Events
{
    public enum UserIsTypingMessageInResultOf
    {
        /// <summary>
        /// Another user is typing a message in destination to the account user
        /// </summary>
        AnotherUserTypingAMessageToAccountUser,

        /// <summary>
        /// This case should not happen and is here in case Twitter changes when they trigger the MessageTyping event.
        /// If you happen to receive this mode, please report to Tweetinvi your case ideally with the associated json.
        /// </summary>
        Unknown
    }

    public class AccountActivityUserIsTypingMessageEventArgs : BaseAccountActivityEventArgs
    {
        public AccountActivityUserIsTypingMessageEventArgs(
            AccountActivityEvent activityEvent,
            IUser sender,
            IUser recipient) 
            : base(activityEvent)
        {
            Sender = sender;
            Recipient = recipient;

            InResultOf = GetInResultOf();
        }

        public IUser Sender { get; }
        public IUser Recipient { get; }
        public UserIsTypingMessageInResultOf InResultOf { get; }

        private UserIsTypingMessageInResultOf GetInResultOf()
        {
            if (Sender.Id != AccountUserId && Recipient.Id == AccountUserId)
            {
                return UserIsTypingMessageInResultOf.AnotherUserTypingAMessageToAccountUser;
            }

            return UserIsTypingMessageInResultOf.Unknown;
        }
    }
}
