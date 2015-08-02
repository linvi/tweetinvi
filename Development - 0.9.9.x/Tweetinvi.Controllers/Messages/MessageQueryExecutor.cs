using System.Collections.Generic;
using Tweetinvi.Core;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Parameters;

namespace Tweetinvi.Controllers.Messages
{
    public interface IMessageQueryExecutor
    {
        // Get messages
        IEnumerable<IMessageDTO> GetLatestMessagesReceived(int maximumMessages = TweetinviConsts.MESSAGE_GET_COUNT);
        IEnumerable<IMessageDTO> GetLatestMessagesReceived(IMessageGetLatestsReceivedRequestParameters queryParameters);

        IEnumerable<IMessageDTO> GetLatestMessagesSent(int maximumMessages = TweetinviConsts.MESSAGE_GET_COUNT);
        IEnumerable<IMessageDTO> GetLatestMessagesSent(IMessageGetLatestsSentRequestParameters queryParameters);

        // Publish Message
        IMessageDTO PublishMessage(IMessageDTO messageDTO);
        IMessageDTO PublishMessage(string messageText, IUserIdentifier targetUserDTO);
        IMessageDTO PublishMessage(string messageText, string targetUserScreenName);
        IMessageDTO PublishMessage(string messageText, long targetUserId);

        // Detroy Message
        bool DestroyMessage(IMessageDTO messageDTO);
        bool DestroyMessage(long messageId);
    }

    public class MessageQueryExecutor : IMessageQueryExecutor
    {
        private readonly ITwitterAccessor _twitterAccessor;
        private readonly IMessageQueryGenerator _messageQueryGenerator;

        public MessageQueryExecutor(
            ITwitterAccessor twitterAccessor,
            IMessageQueryGenerator messageQueryGenerator)
        {
            _twitterAccessor = twitterAccessor;
            _messageQueryGenerator = messageQueryGenerator;
        }

        // Get Messages
        public IEnumerable<IMessageDTO> GetLatestMessagesReceived(int maximumMessages = TweetinviConsts.MESSAGE_GET_COUNT)
        {
            string query = _messageQueryGenerator.GetLatestMessagesReceivedQuery(maximumMessages);
            return _twitterAccessor.ExecuteGETQuery<IEnumerable<IMessageDTO>>(query);
        }

        public IEnumerable<IMessageDTO> GetLatestMessagesReceived(IMessageGetLatestsReceivedRequestParameters queryParameters)
        {
            string query = _messageQueryGenerator.GetLatestMessagesReceivedQuery(queryParameters);
            return _twitterAccessor.ExecuteGETQuery<IEnumerable<IMessageDTO>>(query);
        }

        public IEnumerable<IMessageDTO> GetLatestMessagesSent(int maximumMessages = TweetinviConsts.MESSAGE_GET_COUNT)
        {
            string query = _messageQueryGenerator.GetLatestMessagesSentQuery(maximumMessages);
            return _twitterAccessor.ExecuteGETQuery<IEnumerable<IMessageDTO>>(query);
        }

        public IEnumerable<IMessageDTO> GetLatestMessagesSent(IMessageGetLatestsSentRequestParameters queryParameters)
        {
            string query = _messageQueryGenerator.GetLatestMessagesSentQuery(queryParameters);
            return _twitterAccessor.ExecuteGETQuery<IEnumerable<IMessageDTO>>(query);
        }

        // Publish Message
        public IMessageDTO PublishMessage(IMessageDTO messageDTO)
        {
            string query = _messageQueryGenerator.GetPublishMessageQuery(messageDTO);
            return _twitterAccessor.ExecutePOSTQuery<IMessageDTO>(query);
        }

        public IMessageDTO PublishMessage(string messageText, IUserIdentifier targetUserDTO)
        {
            string query = _messageQueryGenerator.GetPublishMessageQuery(messageText, targetUserDTO);
            return _twitterAccessor.ExecutePOSTQuery<IMessageDTO>(query);
        }

        public IMessageDTO PublishMessage(string messageText, string targetUserScreenName)
        {
            string query = _messageQueryGenerator.GetPublishMessageQuery(messageText, targetUserScreenName);
            return _twitterAccessor.ExecutePOSTQuery<IMessageDTO>(query);
        }

        public IMessageDTO PublishMessage(string messageText, long targetUserId)
        {
            string query = _messageQueryGenerator.GetPublishMessageQuery(messageText, targetUserId);
            return _twitterAccessor.ExecutePOSTQuery<IMessageDTO>(query);
        }

        // Destroy Message
        public bool DestroyMessage(IMessageDTO messageDTO)
        {
            string query = _messageQueryGenerator.GetDestroyMessageQuery(messageDTO);
            return _twitterAccessor.TryExecutePOSTQuery(query);
        }

        public bool DestroyMessage(long messageId)
        {
            string query = _messageQueryGenerator.GetDestroyMessageQuery(messageId);
            return _twitterAccessor.TryExecutePOSTQuery(query);
        }
    }
}