using System;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Messages
{
    public interface IMessageJsonController
    {
        string GetLatestMessages(int count = TweetinviConsts.MESSAGE_GET_COUNT);

        /// <summary>
        /// Warning: Behaviour differs from MessageController.GetLatestMessages.
        /// This method will not make multiple requests to the Twitter API and combine their results,
        /// as that would require parsing the JSON, instead that is left up to the caller.
        /// </summary>
        string GetLatestMessages(IGetMessagesParameters queryParameters);

        // Publish Message
        string PublishMessage(string text, long recipientId);
        string PublishMessage(IPublishMessageParameters parameters);

        // Destroy Message
        bool DestroyMessage(IMessage message);
        bool DestroyMessage(IEventDTO messageDTO);
        bool DestroyMessage(long messageId);
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
        public string GetLatestMessages(int count)
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
        public string GetLatestMessages(IGetMessagesParameters queryParameters)
        {
            string query = _messageQueryGenerator.GetLatestMessagesQuery(queryParameters);
            return _twitterAccessor.ExecuteGETQueryReturningJson(query);
        }

        // Publish Message
        public string PublishMessage(string messageText, long recipientId)
        {
            return PublishMessage(new PublishMessageParameters(messageText, recipientId));
        }

        public string PublishMessage(IPublishMessageParameters parameters)
        {
            string query = _messageQueryGenerator.GetPublishMessageQuery(parameters);
            var reqDTO = _messageQueryGenerator.GetPublishMessageBody(parameters);

            return _twitterAccessor.ExecutePOSTQueryJsonBody(query, reqDTO);
        }

        // Destroy Message
        public bool DestroyMessage(IMessage message)
        {
            if (message == null)
            {
                throw new ArgumentException("Message cannot be null");
            }

            return DestroyMessage(message.EventDTO);
        }

        public bool DestroyMessage(IEventDTO eventDTO)
        {
            string query = _messageQueryGenerator.GetDestroyMessageQuery(eventDTO);
            return _twitterAccessor.TryExecuteDELETEQuery(query);
        }

        public bool DestroyMessage(long messageId)
        {
            string query = _messageQueryGenerator.GetDestroyMessageQuery(messageId);
            return _twitterAccessor.TryExecuteDELETEQuery(query);
        }
    }
}