using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.Events;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Messages
{
    public interface IMessageQueryExecutor
    {
        // Get messages
        Task<IGetMessagesDTO> GetLatestMessages(IGetMessagesParameters queryParameters);

        // Publish Message
        Task<ICreateMessageDTO> PublishMessage(IPublishMessageParameters parameters);

        // Detroy Message
        Task<bool> DestroyMessage(IMessageEventDTO messageDTO);
        Task<bool> DestroyMessage(long messageId);
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
        public Task<IGetMessagesDTO> GetLatestMessages(IGetMessagesParameters queryParameters)
        {
            string query = _messageQueryGenerator.GetLatestMessagesQuery(queryParameters);
            return _twitterAccessor.ExecuteGETQuery<IGetMessagesDTO>(query);
        }

        // Publish Message
        public Task<ICreateMessageDTO> PublishMessage(IPublishMessageParameters parameters)
        {
            string query = _messageQueryGenerator.GetPublishMessageQuery(parameters);
            var reqDTO = _messageQueryGenerator.GetPublishMessageBody(parameters);

            return _twitterAccessor.ExecutePOSTQueryJsonBody<ICreateMessageDTO>(query, reqDTO);
        }

        // Destroy Message
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