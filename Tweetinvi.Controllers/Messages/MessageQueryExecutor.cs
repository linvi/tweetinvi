using Tweetinvi.Core.Web;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Messages
{
    public interface IMessageQueryExecutor
    {
        // Get messages
        IGetMessagesDTO GetLatestMessages(IGetMessagesParameters queryParameters);

        // Publish Message
        ICreateMessageDTO PublishMessage(IPublishMessageParameters parameters);

        // Detroy Message
        bool DestroyMessage(IEventDTO messageDTO);
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
        public IGetMessagesDTO GetLatestMessages(IGetMessagesParameters queryParameters)
        {
            string query = _messageQueryGenerator.GetLatestMessagesQuery(queryParameters);
            return _twitterAccessor.ExecuteGETQuery<IGetMessagesDTO>(query);
        }

        // Publish Message
        public ICreateMessageDTO PublishMessage(IPublishMessageParameters parameters)
        {
            string query = _messageQueryGenerator.GetPublishMessageQuery(parameters);
            var reqDTO = _messageQueryGenerator.GetPublishMessageBody(parameters);

            return _twitterAccessor.ExecutePOSTQueryJsonBody<ICreateMessageDTO>(query, reqDTO);
        }

        // Destroy Message
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