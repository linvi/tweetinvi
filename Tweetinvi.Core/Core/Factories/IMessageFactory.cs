using System.Collections.Generic;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Core.Factories
{
    public interface IMessageFactory
    {
        // Get existing message
        IMessage GetExistingMessage(long messageId);

        // Generate message(s) from DTO from Twitter API
        IMessage GenerateMessageFromGetMessageDTO(IGetMessageDTO getMessageDTO);
        IEnumerable<IMessage> GenerateMessageFromGetMessagesDTO(IGetMessagesDTO getMessagesDTO);
        IMessage GenerateMessageFromCreateMessageDTO(ICreateMessageDTO createMessageDTO);

        // Generate message from DTO (Tweetinvi IEventWithAppDTO)
        IMessage GenerateMessageFromEventWithAppDTO(IEventWithAppDTO eventWithAppDTO);
        IEnumerable<IMessage> GenerateMessagesFromEventWithAppDTOs(IEnumerable<IEventWithAppDTO> eventWithAppDTOs);

        // Generate Message from Json (serialised Tweetinvi IEventWithAppDTO)
        IMessage GenerateMessageFromJson(string jsonMessage);
        IMessage GenerateMessageFromEventDTO(IEventDTO createMessageDTO, IApp app = null);
    }
}