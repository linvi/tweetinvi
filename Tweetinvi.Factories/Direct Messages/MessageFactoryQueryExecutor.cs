using System;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Factories.Properties;

namespace Tweetinvi.Factories
{
    public interface IMessageFactoryQueryExecutor
    {
        // Get Existing Message
        IMessageDTO GetExistingMessage(long messageId);

        // Create Message
        IMessageDTO CreateMessage(string text, IUserDTO recipientDTO);
    }

    public class MessageFactoryQueryExecutor : IMessageFactoryQueryExecutor
    {
        private readonly ITwitterAccessor _twitterAccessor;
        private readonly IFactory<IMessageDTO> _messageDTOUnityFactory;

        public MessageFactoryQueryExecutor(ITwitterAccessor twitterAccessor, IFactory<IMessageDTO> messageDTOUnityFactory)
        {
            _twitterAccessor = twitterAccessor;
            _messageDTOUnityFactory = messageDTOUnityFactory;
        }

        // Get existing message
        public IMessageDTO GetExistingMessage(long messageId)
        {
            string query = string.Format(Resources.Message_GetMessageFromId, messageId);
            return _twitterAccessor.ExecuteGETQuery<IMessageDTO>(query);
        }

        // Create Message
        public IMessageDTO CreateMessage(string text, IUserDTO recipientDTO)
        {
            var messageDTO = _messageDTOUnityFactory.Create();
            messageDTO.Text = text;
            messageDTO.Recipient = recipientDTO;
            return messageDTO;
        }
    }
}