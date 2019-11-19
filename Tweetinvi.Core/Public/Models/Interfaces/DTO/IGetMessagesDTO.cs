using System.Collections.Generic;
using Tweetinvi.Models.DTO.Events;

namespace Tweetinvi.Models.DTO
{
    public interface IGetMessagesDTO
    {
        string NextCursor { get; set; }
        IMessageEventDTO[] MessageEvents { get; set; }
        Dictionary<long, IApp> Apps { get; set; }
    }
}
