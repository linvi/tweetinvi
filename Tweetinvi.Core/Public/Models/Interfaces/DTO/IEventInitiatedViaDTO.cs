using System;
using System.Collections.Generic;
using System.Text;

namespace Tweetinvi.Models.DTO
{
    public interface IEventInitiatedViaDTO
    {
        long? TweetId { get; }
        long? WelcomeMessageId { get; }
    }
}
