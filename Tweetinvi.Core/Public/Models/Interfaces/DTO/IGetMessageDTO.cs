using System.Collections.Generic;

namespace Tweetinvi.Models.DTO
{
    public interface IGetMessageDTO
    {
        IMessageEventDTO MessageEvent { get; set; }
        Dictionary<long, IApp> Apps { get; set; }
    }
}
