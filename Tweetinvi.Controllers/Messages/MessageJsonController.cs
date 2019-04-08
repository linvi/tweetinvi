using System;
using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Messages
{
    public interface IMessageJsonController
    {
        Task<string> GetLatestMessages(int count = TweetinviConsts.MESSAGE_GET_COUNT);

        /// <summary>
        /// Warning: Behaviour differs from MessageController.GetLatestMessages.
        /// This method will not make multiple requests to the Twitter API and combine their results,
        /// as that would require parsing the JSON, instead that is left up to the caller.
        /// </summary>
        Task<string> GetLatestMessages(IGetMessagesParameters queryParameters);

        // Publish Message
        Task<string> PublishMessage(string text, long recipientId);
        Task<string> PublishMessage(IPublishMessageParameters parameters);

        // Destroy Message
        Task<bool> DestroyMessage(IMessage message);
        Task<bool> DestroyMessage(IMessageEventDTO messageDTO);
        Task<bool> DestroyMessage(long messageId);
    }

    public class MessageJsonController : IMessageJsonController
    {
        private readonly IMessageQueryGenerator _messageQueryGenerator;
        private readonly ITwitterAccessor _twitterAccessor;

        public MessageJsonController(
            IMessageQueryGenerator messageQueryGenerator,
            ITwitterAccessor twitterAccessor)
        {
            _messageQueryGenerator = messageQueryGenerator;
            _twitterAccessor = twitterAccessor;
        }

        // Get Messages
        public Task<string> GetLatestMessages(int count)
        {
            var parameters = new GetMessagesParameters()
            {
                Count = count
            };

            return GetLatestMessages(parameters);
        }

        /// <summary>
        /// Warning: Behaviour differs from MessageController.GetLatestMessages.
        /// This method will not make multiple requests to the Twitter API and combine their results,
        /// as that would require parsing the JSON, instead that is left up to the caller.
        /// </summary>
        public Task<string> GetLatestMessages(IGetMessagesParameters queryParameters)
        {
            string query = _messageQueryGenerator.GetLatestMessagesQuery(queryParameters);
            return _twitterAccessor.ExecuteGETQueryReturningJson(query);
        }

        // Publish Message
        public Task<string> PublishMessage(string messageText, long recipientId)
        {
            return PublishMessage(new PublishMessageParameters(messageText, recipientId));
        }

        public Task<string> PublishMessage(IPublishMessageParameters parameters)
        {
            string query = _messageQueryGenerator.GetPublishMessageQuery(parameters);
            var reqDTO = _messageQueryGenerator.GetPublishMessageBody(parameters);

            return _twitterAccessor.ExecutePOSTQueryJsonBody(query, reqDTO);
        }

        // Destroy Message
        public Task<bool> DestroyMessage(IMessage message)
        {
            if (message == null)
            {
                throw new ArgumentException("Message cannot be null");
            }

            return DestroyMessage(message.MessageEventDTO);
        }

        public async Task<bool> DestroyMessage(IMessageEventDTO messageEventDTO)
        {
            string query = _messageQueryGenerator.GetDestroyMessageQuery(messageEventDTO);
            var operationResult = await _twitterAccessor.TryExecuteDELETEQuery(query);

            return operationResult.Success;
        }

        public async Task<bool> DestroyMessage(long messageId)
        {
            string query = _messageQueryGenerator.GetDestroyMessageQuery(messageId);
            var operationResult = await _twitterAccessor.TryExecuteDELETEQuery(query);

            return operationResult.Success;
        }
    }
}