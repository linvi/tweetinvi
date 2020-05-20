using System.Threading.Tasks;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Iterators;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client
{
    public interface IMessagesClient
    {
        /// <summary>
        /// Validate all the message client parameters
        /// </summary>
        IMessagesClientParametersValidator ParametersValidator { get; }

        /// <inheritdoc cref="IMessagesClient.PublishMessageAsync(IPublishMessageParameters)" />
        Task<IMessage> PublishMessageAsync(string text, IUserIdentifier recipient);

        /// <inheritdoc cref="IMessagesClient.PublishMessageAsync(IPublishMessageParameters)" />
        Task<IMessage> PublishMessageAsync(string text, long recipientId);

        /// <summary>
        /// Publishes a private message
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/direct-messages/sending-and-receiving/api-reference/new-event </para>
        /// <returns>Message published</returns>
        Task<IMessage> PublishMessageAsync(IPublishMessageParameters parameters);

        /// <inheritdoc cref="IMessagesClient.GetMessageAsync(IGetMessageParameters)" />
        Task<IMessage> GetMessageAsync(long messageId);

        /// <summary>
        /// Gets a specific message
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/direct-messages/sending-and-receiving/api-reference/get-event </para>
        /// <returns>Requested message</returns>
        Task<IMessage> GetMessageAsync(IGetMessageParameters parameters);

        /// <inheritdoc cref="IMessagesClient.GetMessagesAsync(IGetMessagesParameters)" />
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

        /// <inheritdoc cref="IMessagesClient.DestroyMessageAsync(IDeleteMessageParameters)" />
        Task DestroyMessageAsync(long messageId);
        /// <inheritdoc cref="IMessagesClient.DestroyMessageAsync(IDeleteMessageParameters)" />
        Task DestroyMessageAsync(IMessage message);

        /// <summary>
        /// Destroy a specific message
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/direct-messages/sending-and-receiving/guides/direct-message-migration </para>
        Task DestroyMessageAsync(IDeleteMessageParameters parameters);
    }
}