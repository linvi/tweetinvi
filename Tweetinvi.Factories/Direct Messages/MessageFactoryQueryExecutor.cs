using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Web;
using Tweetinvi.Factories.Properties;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Factories
{
    public interface IMessageFactoryQueryExecutor
    {
        // Get Existing Message
        IGetMessageDTO GetExistingMessage(long messageId);
    }

    public class MessageFactoryQueryExecutor : IMessageFactoryQueryExecutor
    {
        private readonly ITwitterAccessor _twitterAccessor;

        public MessageFactoryQueryExecutor(ITwitterAccessor twitterAccessor)
        {
            _twitterAccessor = twitterAccessor;
        }

        // Get existing message
        public IGetMessageDTO GetExistingMessage(long messageId)
        {
            string query = string.Format(Resources.Message_GetMessageFromId, messageId);
            return _twitterAccessor.ExecuteGETQuery<IGetMessageDTO>(query);
        }
    }
}