using Tweetinvi.Models;

namespace Tweetinvi.Events
{
    public class BaseAccountActivityMessageEventArgs : BaseAccountActivityEventArgs
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

        public IMessage Message { get; }
        public IUser Sender { get; }
        public IUser Recipient { get; }
        public IApp App { get; }
    }
}
