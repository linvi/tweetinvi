using System.Threading.Tasks;
using Tweetinvi.Client.Requesters;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client
{
    public class MessageClient : IMessageClient
    {
        private readonly IMessageRequester _messageRequester;

        public MessageClient(IMessageRequester messageRequester)
        {
            _messageRequester = messageRequester;
        }

        public Task<IMessage> PublishMessage(string text, long? recipientId)
        {
            return PublishMessage(new PublishMessageParameters(text, recipientId));
        }

        public async Task<IMessage> PublishMessage(IPublishMessageParameters parameters)
        {
            var twitterResult = await _messageRequester.PublishMessage(parameters).ConfigureAwait(false);
            return twitterResult?.Result;
        }

        public Task<IMessage> GetMessage(long messageId)
        {
            return GetMessage(new GetMessageParameters(messageId));
        }

        public async Task<IMessage> GetMessage(IGetMessageParameters parameters)
        {
            var twitterResult = await _messageRequester.GetMessage(parameters).ConfigureAwait(false);
            return twitterResult?.Result;
        }

        public Task DestroyMessage(long messageId)
        {
            return DestroyMessage(new DestroyMessageParameters(messageId));
        }

        public Task DestroyMessage(IMessage message)
        {
            return DestroyMessage(new DestroyMessageParameters(message));
        }

        public async Task DestroyMessage(IDeleteMessageParameters parameters)
        {
            await _messageRequester.DestroyMessage(parameters).ConfigureAwait(false);
        }
    }
}