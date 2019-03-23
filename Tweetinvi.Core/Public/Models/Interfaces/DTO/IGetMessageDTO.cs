using System.Collections.Generic;

namespace Tweetinvi.Models.DTO
{
    public interface IGetMessageDTO
    {
        IEventDTO Event { get; set; }
        Dictionary<long, IApp> Apps { get; set; }
    }
}
