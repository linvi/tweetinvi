using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Client.Tools;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.Events;

namespace Tweetinvi.Factories
{
    public class MessageFactory : IMessageFactory
    {
        private readonly IMessageFactoryQueryExecutor _messageFactoryQueryExecutor;
        private readonly ITwitterClientFactories _factories;

        public MessageFactory(
            IMessageFactoryQueryExecutor messageFactoryQueryExecutor,
            ITwitterClientFactories factories)
        {
            _messageFactoryQueryExecutor = messageFactoryQueryExecutor;
            _factories = factories;
        }

        // Get existing message
        public async Task<IMessage> GetExistingMessage(long messageId)
        {
            var getMessageDTO = await _messageFactoryQueryExecutor.GetExistingMessage(messageId);
            return _factories.CreateMessage(getMessageDTO);
        }
    }
}