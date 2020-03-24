using System.Collections.Generic;
using Tweetinvi.Models.DTO.Events;

namespace Tweetinvi.Models.DTO
{
    public interface IGetMessageDTO
    {
        IMessageEventDTO MessageEvent { get; set; }
        Dictionary<long, IApp> Apps { get; set; }
    }
}
