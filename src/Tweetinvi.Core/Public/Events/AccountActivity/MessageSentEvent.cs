using Tweetinvi.Models;

namespace Tweetinvi.Events
{
    public enum MessageSentInResultOf
    {
        /// <summary>
        /// This case should not happen and is here in case Twitter changes when they trigger the MessageCreated event.
        /// If you happen to receive this mode, please report to Tweetinvi your case ideally with the associated json.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The account user sent a message.
        /// </summary>
        AccountUserSendingAMessage,
    }

    /// <summary>
    /// Event information when a message is published.
    /// </summary>
    public class MessageSentEvent : BaseAccountActivityMessageEventArgs<MessageSentInResultOf>
    {
        public MessageSentEvent(
            AccountActivityEvent activityEvent,
            IMessage message,
            IUser sender,
            IUser recipient,
            IApp app) : base(activityEvent, message, sender, recipient, app)
        {
            InResultOf = GetInResultOf();
        }

        private MessageSentInResultOf GetInResultOf()
        {
            if (Message.SenderId == AccountUserId)
            {
                return MessageSentInResultOf.AccountUserSendingAMessage;
            }

            return MessageSentInResultOf.Unknown;
        }
    }
}
