using System;
using System.Collections.Generic;
using System.Text;

namespace Tweetinvi.Models.DTO
{
    /// <summary>
    /// DTO for encapsulating an Event and an App together for storage.
    /// Not used for transfer to or from Twitter.
    /// </summary>
    public interface IEventWithAppDTO
    {
        IEventDTO Event { get; set; }
        IApp App { get; set; }
    }
}
