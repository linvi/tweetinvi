using System;
using System.Collections.Generic;
using System.Text;

namespace Tweetinvi.Models.DTO
{
    public interface IMessageCreateDTO
    {
        // Tweetinvi fields
        bool IsDestroyed { get; set; }

        // Twitter fields
        IMessageCreateTargetDTO Target { get; set; }
        long SenderId { get; set; }
        long? SourceAppId { get; set; }
        IMessageDataDTO MessageData { get; set; }
    }
}
