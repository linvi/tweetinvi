using Tweetinvi.Models;

namespace Tweetinvi.Events
{
    public enum UserIsTypingMessageInResultOf
    {
        /// <summary>
        /// This case should not happen and is here in case Twitter changes when they trigger the MessageTyping event.
        /// If you happen to receive this mode, please report to Tweetinvi your case ideally with the associated json.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Another user is typing a message in destination to the account user
        /// </summary>
        AnotherUserTypingAMessageToAccountUser,
    }

    /// <summary>
    /// Event information when a user is typing in a private message conversation.
    /// </summary>
    public class UserIsTypingMessageEvent : BaseAccountActivityEventArgs<UserIsTypingMessageInResultOf>
    {
        public UserIsTypingMessageEvent(
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
