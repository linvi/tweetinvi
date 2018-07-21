using System;
using System.Collections.Generic;
using System.Text;

namespace Tweetinvi.Models.DTO
{
    public interface IGetMessageDTO
    {
        IEventDTO Event { get; set; }
        Dictionary<long, IApp> Apps { get; set; }
    }
}
