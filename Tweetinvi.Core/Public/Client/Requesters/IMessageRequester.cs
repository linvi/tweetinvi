using System.Threading.Tasks;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client.Requesters
{
    public interface IMessageRequester
    {
        /// <summary>
        /// Publishes a private message
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/direct-messages/sending-and-receiving/api-reference/new-event </para>
        /// <returns>Twitter result containing the published message</returns>
        Task<ITwitterResult<ICreateMessageDTO>> PublishMessage(IPublishMessageParameters parameters);

        /// <summary>
        /// Destroy a specific message
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/direct-messages/sending-and-receiving/guides/direct-message-migration </para>
        Task<ITwitterResult> DestroyMessage(IDeleteMessageParameters parameters);

        /// <summary>
        /// Gets a specific message
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/direct-messages/sending-and-receiving/api-reference/get-event </para>
        /// <returns>Twitter result containing the requested message</returns>
        Task<ITwitterResult<IGetMessageDTO>> GetMessage(IGetMessageParameters parameters);

        /// <summary>
        /// Gets latest messages
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/direct-messages/sending-and-receiving/api-reference/list-events </para>
        /// <returns>An iterator to list the recent messages of the user</returns>
        ITwitterPageIterator<ITwitterResult<IMessageCursorQueryResultDTO>> GetMessagesIterator(IGetMessagesParameters parameters);
    }
}