using System;
using Tweetinvi.Core.Interfaces.Async;
using Tweetinvi.Core.Interfaces.DTO;

namespace Tweetinvi.Core.Interfaces
{
    /// <summary>
    /// Message that can be sent privately between Twitter users
    /// </summary>
    public interface IMessage : IMessageAsync, IEquatable<IMessage>
    {
        IMessageDTO MessageDTO { get; set; }

        bool IsMessagePublished { get; }
        bool IsMessageDestroyed { get; }

        /// <summary>
        /// Id of the Message
        /// </summary>
        long Id { get; }

        /// <summary>
        /// Text contained in the message
        /// </summary>
        string Text { get; }

        /// <summary>
        /// Creation date of the message
        /// </summary>
        DateTime CreatedAt { get; }

        long SenderId { get; }
        string SenderScreenName { get; }

        /// <summary>
        /// User who sent the message
        /// </summary>
        IUser Sender { get; }

        long RecipientId { get; }
        string RecipientScreenName { get; }

        /// <summary>
        /// Recipient of the message
        /// </summary>
        IUser Recipient { get; }

        bool Destroy();

        void SetRecipient(IUser recipient);
    }
}