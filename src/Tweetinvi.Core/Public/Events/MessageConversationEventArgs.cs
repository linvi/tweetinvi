using System;
using Tweetinvi.Models;

namespace Tweetinvi.Events
{
    public class MessageConversationEventArgs : EventArgs
    {
        public long SenderId { get; set; }
        public long RecipientId { get; set; }

        public IUser Sender { get; set; }
        public IUser Recipient { get; set; }
    }
}
