using System;

namespace Tweetinvi.Models.DTO
{
    public interface IEventDTO
    {
        EventType Type { get; set; }
        long Id { get; set; }
        DateTime CreatedAt { get; set; }
        IEventInitiatedViaDTO InitiatedVia { get; set; }
        IMessageCreateDTO MessageCreate { get; set; }
    }
}
