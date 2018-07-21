using System.Collections.Generic;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Controllers
{
    public interface IMessageController
    {
        IEnumerable<IMessage> GetLatestMessages(int count = TweetinviConsts.MESSAGE_GET_COUNT);
        IEnumerable<IMessage> GetLatestMessages(int count, out string cursor);
        IEnumerable<IMessage> GetLatestMessages(IGetMessagesParameters getMessagesParameters);
        IEnumerable<IMessage> GetLatestMessages(IGetMessagesParameters getMessagesParameters, out string cursor);

        // Publish Message
        IMessage PublishMessage(string text, long recipientId);
        IMessage PublishMessage(IPublishMessageParameters parameters);

        // Destroy Message
        bool DestroyMessage(IMessage message);
        bool DestroyMessage(IEventDTO eventDTO);
        bool DestroyMessage(long messageId);
    }
}