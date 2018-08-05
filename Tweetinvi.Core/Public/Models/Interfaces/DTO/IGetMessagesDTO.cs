using System.Collections.Generic;

namespace Tweetinvi.Models.DTO
{
    public interface IGetMessagesDTO
    {
        string NextCursor { get; set; }
        IEventDTO[] Events { get; set; }
        Dictionary<long, IApp> Apps { get; set; }
    }
}
