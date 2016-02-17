using System;
using Tweetinvi.Core;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Controllers.Messages
{
    public interface IMessageJsonController
    {
        string GetLatestMessagesReceived(int maximumMessages = TweetinviConsts.MESSAGE_GET_COUNT);
        string GetLatestMessagesReceived(IMessagesReceivedParameters queryParameters);

        string GetLatestMessagesSent(int maximumMessages = TweetinviConsts.MESSAGE_GET_COUNT);
        string GetLatestMessagesSent(IMessagesSentParameters queryParameters);

        // Publish Message
        string PublishMessage(IMessage message);
        string PublishMessage(IMessageDTO message);
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
            string query = _messageQueryGenerator.GetLatestMessagesReceivedQuery(maximumMessages);
            return _twitterAccessor.ExecuteJsonGETQuery(query);
        }

        public string GetLatestMessagesReceived(IMessagesReceivedParameters queryParameters)
        {
            string query = _messageQueryGenerator.GetLatestMessagesReceivedQuery(queryParameters);
            return _twitterAccessor.ExecuteJsonGETQuery(query);
        }

        public string GetLatestMessagesSent(int maximumMessages = TweetinviConsts.MESSAGE_GET_COUNT)
        {
            string query = _messageQueryGenerator.GetLatestMessagesSentQuery(maximumMessages);
            return _twitterAccessor.ExecuteJsonGETQuery(query);
        }

        public string GetLatestMessagesSent(IMessagesSentParameters queryParameters)
        {
            string query = _messageQueryGenerator.GetLatestMessagesSentQuery(queryParameters);
            return _twitterAccessor.ExecuteJsonGETQuery(query);
        }

        // Publish Message
        public string PublishMessage(IMessage message)
        {
            return PublishMessage(new PublishMessageParameters(message));
        }

        public string PublishMessage(IMessageDTO message)
        {
            return PublishMessage(new PublishMessageParameters(message));
        }

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
            return _twitterAccessor.ExecuteJsonPOSTQuery(query);
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
            return _twitterAccessor.ExecuteJsonPOSTQuery(query);
        }

        public string DestroyMessage(long messageId)
        {
            string query = _messageQueryGenerator.GetDestroyMessageQuery(messageId);
            return _twitterAccessor.ExecuteJsonPOSTQuery(query);
        }
    }
}