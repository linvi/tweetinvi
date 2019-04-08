using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Core.Factories
{
    public interface IMessageFactory
    {
        // Get existing message
        Task<IMessage> GetExistingMessage(long messageId);

        // Generate message(s) from DTO from Twitter API
        IMessage GenerateMessageFromGetMessageDTO(IGetMessageDTO getMessageDTO);
        IEnumerable<IMessage> GenerateMessageFromGetMessagesDTO(IGetMessagesDTO getMessagesDTO);
        IMessage GenerateMessageFromCreateMessageDTO(ICreateMessageDTO createMessageDTO);

        // Generate message from DTO (Tweetinvi IMessageEventWithAppDTO)
        IMessage GenerateMessageFromEventWithAppDTO(IMessageEventWithAppDTO messageEventWithAppDTO);
        IEnumerable<IMessage> GenerateMessagesFromEventWithAppDTOs(IEnumerable<IMessageEventWithAppDTO> eventWithAppDTOs);

        // Generate Message from Json (serialised Tweetinvi IMessageEventWithAppDTO)
        IMessage GenerateMessageFromJson(string jsonMessage);
        IMessage GenerateMessageFromEventDTO(IMessageEventDTO createMessageDTO, IApp app = null);
    }
}