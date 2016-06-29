using System;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Messages
{
    public interface IMessageJsonController
    {
        string GetLatestMessagesReceived(int maximumMessages = TweetinviConsts.MESSAGE_GET_COUNT);
        string GetLatestMessagesReceived(IMessagesReceivedParameters queryParameters);

        string GetLatestMessagesSent(int maximumMessages = TweetinviConsts.MESSAGE_GET_COUNT);
        string GetLatestMessagesSent(IMessagesSentParameters queryParameters);

        // Publish Message
        string PublishMessage(string text, IUserIdentifier recipient);
        string PublishMessage(string text, string recipientScreenName);
        string PublishMessage(string text, long recipientId);

        // Destroy Message
        string DestroyMessage(IMessage message);
        string DestroyMessage(IMessageDTO messageDTO);
        string DestroyMessage(long messageId);
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
        public string GetLatestMessagesReceived(int maximumMessages = TweetinviConsts.MESSAGE_GET_COUNT)
        {
            var parameter = new MessagesReceivedParameters
            {
                MaximumNumberOfMessagesToRetrieve = maximumMessages
            };

            string query = _messageQueryGenerator.GetLatestMessagesReceivedQuery(parameter);
            return _twitterAccessor.ExecuteGETQueryReturningJson(query);
        }

        public string GetLatestMessagesReceived(IMessagesReceivedParameters queryParameters)
        {
            string query = _messageQueryGenerator.GetLatestMessagesReceivedQuery(queryParameters);
            return _twitterAccessor.ExecuteGETQueryReturningJson(query);
        }

        public string GetLatestMessagesSent(int maximumMessages = TweetinviConsts.MESSAGE_GET_COUNT)
        {
            var parameter = new MessagesSentParameters()
            {
                MaximumNumberOfMessagesToRetrieve = maximumMessages
            };

            string query = _messageQueryGenerator.GetLatestMessagesSentQuery(parameter);
            return _twitterAccessor.ExecuteGETQueryReturningJson(query);
        }

        public string GetLatestMessagesSent(IMessagesSentParameters queryParameters)
        {
            string query = _messageQueryGenerator.GetLatestMessagesSentQuery(queryParameters);
            return _twitterAccessor.ExecuteGETQueryReturningJson(query);
        }

        // Publish Message

        public string PublishMessage(string messageText, string recipientScreenName)
        {
            return PublishMessage(new PublishMessageParameters(messageText, recipientScreenName));
        }

        public string PublishMessage(string messageText, long recipientId)
        {
            return PublishMessage(new PublishMessageParameters(messageText, recipientId));
        }

        public string PublishMessage(string messageText, IUserIdentifier recipient)
        {
            return PublishMessage(new PublishMessageParameters(messageText, recipient));
        }

        public string PublishMessage(IPublishMessageParameters parameters)
        {
            string query = _messageQueryGenerator.GetPublishMessageQuery(parameters);
            return _twitterAccessor.ExecutePOSTQueryReturningJson(query);
        }

        // Destroy Message
        public string DestroyMessage(IMessage message)
        {
            if (message == null)
            {
                throw new ArgumentException("Message cannot be null");
            }

            return DestroyMessage(message.MessageDTO);
        }

        public string DestroyMessage(IMessageDTO messageDTO)
        {
            string query = _messageQueryGenerator.GetDestroyMessageQuery(messageDTO);
            return _twitterAccessor.ExecutePOSTQueryReturningJson(query);
        }

        public string DestroyMessage(long messageId)
        {
            string query = _messageQueryGenerator.GetDestroyMessageQuery(messageId);
            return _twitterAccessor.ExecutePOSTQueryReturningJson(query);
        }
    }
}