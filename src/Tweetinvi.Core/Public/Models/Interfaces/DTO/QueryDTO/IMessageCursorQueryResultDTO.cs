using System.Collections.Generic;
using Tweetinvi.Models.DTO.Events;

namespace Tweetinvi.Models.DTO.QueryDTO
{
    public interface IMessageCursorQueryResultDTO : IBaseCursorQueryDTO<IMessageEventDTO>
    {
        IMessageEventDTO[] MessageEvents { get; set; }
        Dictionary<long, IApp> Apps { get; set; }
    }
}
