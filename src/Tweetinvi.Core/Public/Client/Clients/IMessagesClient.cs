using System.Threading.Tasks;
using Tweetinvi.Iterators;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client
{
    public interface IMessagesClient
    {
        /// <inheritdoc cref="PublishMessage(IPublishMessageParameters)" />
        Task<IMessage> PublishMessageAsync(string text, IUserIdentifier recipient);

        /// <inheritdoc cref="PublishMessage(IPublishMessageParameters)" />
        Task<IMessage> PublishMessageAsync(string text, long recipientId);

        /// <summary>
        /// Publishes a private message
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/direct-messages/sending-and-receiving/api-reference/new-event </para>
        /// <returns>Message published</returns>
        Task<IMessage> PublishMessageAsync(IPublishMessageParameters parameters);

        /// <inheritdoc cref="GetMessage(IGetMessageParameters)" />
        Task<IMessage> GetMessageAsync(long messageId);

        /// <summary>
        /// Gets a specific message
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/direct-messages/sending-and-receiving/api-reference/get-event </para>
        /// <returns>Requested message</returns>
        Task<IMessage> GetMessageAsync(IGetMessageParameters parameters);

        /// <inheritdoc cref="GetMessages(IGetMessagesParameters)" />
        Task<IMessage[]> GetMessagesAsync();

        /// <summary>
        /// Gets latest messages
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/direct-messages/sending-and-receiving/api-reference/list-events </para>
        /// <returns>List the recent messages of the user</returns>
        Task<IMessage[]> GetMessagesAsync(IGetMessagesParameters parameters);

        /// <inheritdoc cref="GetMessagesIterator(IGetMessagesParameters)" />
        ITwitterIterator<IMessage> GetMessagesIterator();

        /// <summary>
        /// Gets latest messages
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/direct-messages/sending-and-receiving/api-reference/list-events </para>
        /// <returns>An iterator to list the recent messages of the user</returns>
        ITwitterIterator<IMessage> GetMessagesIterator(IGetMessagesParameters parameters);

        /// <inheritdoc cref="DestroyMessage(IDeleteMessageParameters)" />
        Task DestroyMessageAsync(long messageId);
        /// <inheritdoc cref="DestroyMessage(IDeleteMessageParameters)" />
        Task DestroyMessageAsync(IMessage message);

        /// <summary>
        /// Destroy a specific message
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/direct-messages/sending-and-receiving/guides/direct-message-migration </para>
        Task DestroyMessageAsync(IDeleteMessageParameters parameters);
    }
}