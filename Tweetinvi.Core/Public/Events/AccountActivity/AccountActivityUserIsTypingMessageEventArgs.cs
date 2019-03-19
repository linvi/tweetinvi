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

    public class AccountActivityUserIsTypingMessageEventArgs : BaseAccountActivityEventArgs<UserIsTypingMessageInResultOf>
    {
        public AccountActivityUserIsTypingMessageEventArgs(
            AccountActivityEvent activityEvent,
            IUser typingUser,
            IUser typingTo) 
            : base(activityEvent)
        {
            TypingUser = typingUser;
            TypingTo = typingTo;

            InResultOf = GetInResultOf();
        }

        /// <summary>
        /// The user who is typing
        /// </summary>
        public IUser TypingUser { get; }

        /// <summary>
        /// The user who is going to receive a message
        /// </summary>
        public IUser TypingTo { get; }

        private UserIsTypingMessageInResultOf GetInResultOf()
        {
            if (TypingUser.Id != AccountUserId && TypingTo.Id == AccountUserId)
            {
                return UserIsTypingMessageInResultOf.AnotherUserTypingAMessageToAccountUser;
            }

            return UserIsTypingMessageInResultOf.Unknown;
        }
    }
}
