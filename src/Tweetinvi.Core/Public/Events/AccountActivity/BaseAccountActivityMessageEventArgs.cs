using Tweetinvi.Models;

namespace Tweetinvi.Events
{
    /// <summary>
    /// Event information when a message is published.
    /// </summary>
    public class BaseAccountActivityMessageEventArgs<T> : BaseAccountActivityEventArgs<T>
    {
        protected BaseAccountActivityMessageEventArgs(
            AccountActivityEvent activityEvent,
            IMessage message,
            IUser sender,
            IUser recipient,
            IApp app) : base(activityEvent)
        {
            Message = message;
            Sender = sender;
            Recipient = recipient;
            App = app;
        }

        /// <summary>
        /// Message for which the event has occurred
        /// </summary>
        public IMessage Message { get; }

        /// <summary>
        /// Send of the message
        /// </summary>
        public IUser Sender { get; }

        /// <summary>
        /// Recipient of the message
        /// </summary>
        public IUser Recipient { get; }

        /// <summary>
        /// Application used to send the message
        /// </summary>
        public IApp App { get; }
    }
}
