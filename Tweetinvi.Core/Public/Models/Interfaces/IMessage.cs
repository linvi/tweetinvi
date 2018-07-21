using System;
using Tweetinvi.Core.Models.Async;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.Entities;

namespace Tweetinvi.Models
{
    /// <summary>
    /// Message that can be sent privately between Twitter users privately.
    /// https://developer.twitter.com/en/docs/direct-messages/sending-and-receiving/api-reference/new-event
    /// </summary>
    public interface IMessage : IMessageAsync, IEquatable<IMessage>
    {
        /// <summary>
        /// Underlying DTO representing this message (as an event).
        /// </summary>
        IEventDTO EventDTO { get; }

        /// <summary>
        /// The App that was used to send this message.
        /// If this message is a response to creating it, the app will be null and you can set it here yourself if required.
        /// </summary>
        IApp App { get; set; }

        /// <summary>
        /// Informs if the message has been destroyed.
        /// </summary>
        bool IsDestroyed { get; }

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
        /// ID of the user who received the message.
        /// </summary>
        long RecipientId { get; }

        /// <summary>
        /// Entities of the message.
        /// </summary>
        IMessageEntities Entities { get; }

        /// <summary>
        /// The ID of the Tweet with Direct Message Prompt the conversation was initiated from if one was used.
        /// </summary>
        long? InitiatedViaTweetId { get; }

        /// <summary>
        /// The ID of the Welcome Message immediatley preceding the conversation if one was used.
        /// </summary>
        long? InitiatedViaWelcomeMessageId { get; }

        /// <summary>
        /// The Quick reply response that the user selected (if any), triggering this message.
        /// </summary>
        IQuickReplyResponse QuickReplyResponse { get; }

        /// <summary>
        /// Media that was attached to the message.
        /// </summary>
        IMediaEntity AttachedMedia { get; }

        /// <summary>
        /// Destroy the message.
        /// </summary>
        bool Destroy();
    }
}