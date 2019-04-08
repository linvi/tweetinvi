using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Models;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Controllers
{
    public interface IMessageController
    {
        Task<AsyncCursorResult<IEnumerable<IMessage>>> GetLatestMessages(int count = TweetinviConsts.MESSAGE_GET_COUNT);
        Task<AsyncCursorResult<IEnumerable<IMessage>>> GetLatestMessages(IGetMessagesParameters getMessagesParameters);

        // Publish Message
        Task<IMessage> PublishMessage(string text, long recipientId);
        Task<IMessage> PublishMessage(IPublishMessageParameters parameters);

        // Destroy Message
        Task<bool> DestroyMessage(IMessage message);
        Task<bool> DestroyMessage(IMessageEventDTO messageEventDTO);
        Task<bool> DestroyMessage(long messageId);
    }
}