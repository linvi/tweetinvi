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
        private readonly IFactory<IMessage> _messageFactory;
        private readonly ITwitterClientFactories _factories;
        private readonly IJsonObjectConverter _jsonObjectConverter;

        public MessageFactory(
            IMessageFactoryQueryExecutor messageFactoryQueryExecutor,
            IFactory<IMessage> messageFactory,
            ITwitterClientFactories factories,
            IJsonObjectConverter jsonObjectConverter)
        {
            _messageFactoryQueryExecutor = messageFactoryQueryExecutor;
            _messageFactory = messageFactory;
            _factories = factories;
            _jsonObjectConverter = jsonObjectConverter;
        }

        // Get existing message
        public async Task<IMessage> GetExistingMessage(long messageId)
        {
            var getMessageDTO = await _messageFactoryQueryExecutor.GetExistingMessage(messageId);
            return _factories.CreateMessage(getMessageDTO);
        }
    }
}