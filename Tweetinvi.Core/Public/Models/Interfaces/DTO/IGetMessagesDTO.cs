using System;
using System.Collections.Generic;
using System.Text;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Models.DTO
{
    public interface IGetMessagesDTO
    {
        string NextCursor { get; set; }
        IEventDTO[] Events { get; set; }
        Dictionary<long, IApp> Apps { get; set; }
    }
}
