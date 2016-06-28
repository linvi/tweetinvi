using System.Collections.Generic;
using Tweetinvi.Core.Web;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Messages
{
    public interface IMessageQueryExecutor
    {
        // Get messages
        IEnumerable<IMessageDTO> GetLatestMessagesReceived(IMessagesReceivedParameters queryParameters);
        IEnumerable<IMessageDTO> GetLatestMessagesSent(IMessagesSentParameters queryParameters);

        // Publish Message
        IMessageDTO PublishMessage(IPublishMessageParameters parameters);

        // Detroy Message
        bool DestroyMessage(IMessageDTO messageDTO);
        bool DestroyMessage(long messageId);
    }

    public class MessageQueryExecutor : IMessageQueryExecutor
    {
        private readonly ITwitterAccessor _twitterAccessor;
        private readonly IMessageQueryValidator _messageQueryValidator;
        private readonly IMessageQueryGenerator _messageQueryGenerator;

        public MessageQueryExecutor(
            ITwitterAccessor twitterAccessor,
            IMessageQueryValidator messageQueryValidator,
            IMessageQueryGenerator messageQueryGenerator)
        {
            _twitterAccessor = twitterAccessor;
            _messageQueryValidator = messageQueryValidator;
            _messageQueryGenerator = messageQueryGenerator;
        }

        // Get Messages
        public IEnumerable<IMessageDTO> GetLatestMessagesReceived(IMessagesReceivedParameters queryParameters)
        {
            string query = _messageQueryGenerator.GetLatestMessagesReceivedQuery(queryParameters);
            return _twitterAccessor.ExecuteGETQuery<IEnumerable<IMessageDTO>>(query);
        }

      
        public IEnumerable<IMessageDTO> GetLatestMessagesSent(IMessagesSentParameters queryParameters)
        {
            string query = _messageQueryGenerator.GetLatestMessagesSentQuery(queryParameters);
            return _twitterAccessor.ExecuteGETQuery<IEnumerable<IMessageDTO>>(query);
        }

        // Publish Message
        public IMessageDTO PublishMessage(IPublishMessageParameters parameters)
        {
            _messageQueryValidator.ThrowIfMessageCannotBePublished(parameters);

            string query = _messageQueryGenerator.GetPublishMessageQuery(parameters);
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