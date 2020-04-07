using Tweetinvi.Models;

namespace Tweetinvi.Events
{
    public enum MessageReceivedInResultOf
    {
        /// <summary>
        /// This case should not happen and is here in case Twitter changes when they trigger the MessageCreated event.
        /// If you happen to receive this mode, please report to Tweetinvi your case ideally with the associated json.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The account user received a message.
        /// </summary>
        AccountUserReceivingAMessage,
    }

    /// <summary>
    /// Event information when a message is published.
    /// </summary>
    public class MessageReceivedEvent : BaseAccountActivityMessageEventArgs<MessageReceivedInResultOf>
    {
        public MessageReceivedEvent(
            AccountActivityEvent activityEvent,
            IMessage message,
            IUser sender,
            IUser recipient,
            IApp app) : base(activityEvent, message, sender, recipient, app)
        {
            InResultOf = GetInResultOf();
        }

        private MessageReceivedInResultOf GetInResultOf()
        {
            if (Message.RecipientId == AccountUserId)
            {
                return MessageReceivedInResultOf.AccountUserReceivingAMessage;
            }

            return MessageReceivedInResultOf.Unknown;
        }
    }
}
