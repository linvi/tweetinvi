using Tweetinvi.Models;

namespace Tweetinvi.Events
{
    public enum UserReadMessageConversationInResultOf
    {
        /// <summary>
        /// This case should not happen and is here in case Twitter changes when they trigger the MessageRead event.
        /// If you happen to receive this mode, please report to Tweetinvi your case ideally with the associated json.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Another user has read a message sent by the account user
        /// </summary>
        AnotherUserReadingConversation,
    }

    /// <summary>
    /// Event information when a user reads a private message conversation.
    /// </summary>
    public class UserReadMessageConversationEvent : BaseAccountActivityEventArgs<UserReadMessageConversationInResultOf>
    {
        public UserReadMessageConversationEvent(
            AccountActivityEvent activityEvent,
            IUser userWhoReadTheMessageConversation,
            IUser userWhoWroteTheMessage,
            string lastReadEventId)
            : base(activityEvent)
        {
            UserWhoReadTheMessageConversation = userWhoReadTheMessageConversation;
            UserWhoWroteTheMessage = userWhoWroteTheMessage;
            LastReadEventId = lastReadEventId;

            InResultOf = GetInResultOf();
        }

        /// <summary>
        /// The user who read the message
        /// </summary>
        public IUser UserWhoReadTheMessageConversation { get; }

        /// <summary>
        /// The user who sent the message that just got read
        /// </summary>
        public IUser UserWhoWroteTheMessage { get; }

        /// <summary>
        /// An identifier of the read action
        /// </summary>
        public string LastReadEventId { get; }

        private UserReadMessageConversationInResultOf GetInResultOf()
        {
            if (UserWhoReadTheMessageConversation.Id != AccountUserId && UserWhoWroteTheMessage.Id == AccountUserId)
            {
                return UserReadMessageConversationInResultOf.AnotherUserReadingConversation;
            }

            return UserReadMessageConversationInResultOf.Unknown;
        }
    }
}
