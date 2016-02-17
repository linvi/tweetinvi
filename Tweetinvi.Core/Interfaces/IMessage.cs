using System;
using Tweetinvi.Core.Interfaces.Async;
using Tweetinvi.Core.Interfaces.DTO;

namespace Tweetinvi.Core.Interfaces
{
    /// <summary>
    /// Message that can be sent privately between Twitter users privately.
    /// https://dev.twitter.com/rest/reference/post/direct_messages/new
    /// </summary>
    public interface IMessage : IMessageAsync, IEquatable<IMessage>
    {
        /// <summary>
        /// Property storing the message data.
        /// </summary>
        IMessageDTO MessageDTO { get; set; }

        /// <summary>
        /// Informs if the message has already been published.
        /// </summary>
        bool IsMessagePublished { get; }

        /// <summary>
        /// Informs if the message has been destroyed.
        /// </summary>
        bool IsMessageDestroyed { get; }

        /// <summary>
        /// Id of the Message.
        /// </summary>
        long Id { get; }

        /// <summary>
        /// Text contained in the message.
        /// </summary>
        string Text { get; }

        /// <summary>
        /// Creation date of the message.
        /// </summary>
        DateTime CreatedAt { get; }

        /// <summary>
        /// Id of the user who sent the message.
        /// </summary>
        long SenderId { get; }

        /// <summary>
        /// Screen name of the user who sent the message.
        /// </summary>
        string SenderScreenName { get; }

        /// <summary>
        /// User who sent the message.
        /// </summary>
        IUser Sender { get; }

        /// <summary>
        /// Id of the user who received the message.
        /// </summary>
        long RecipientId { get; }

        /// <summary>
        /// Screen name of the user who received the message.
        /// </summary>
        string RecipientScreenName { get; }

        /// <summary>
        /// User who received the message.
        /// </summary>
        IUser Recipient { get; }

        /// <summary>
        /// Destroy the message.
        /// </summary>
        bool Destroy();

        /// <summary>
        /// Set the recipient to a message that has not yet been published.
        /// </summary>
        void SetRecipient(IUser recipient);
    }
}