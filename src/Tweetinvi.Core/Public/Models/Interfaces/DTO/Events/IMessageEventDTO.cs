using System;

namespace Tweetinvi.Models.DTO.Events
{
    public interface IMessageEventDTO
    {
        EventType Type { get; set; }
        long Id { get; set; }
        DateTimeOffset CreatedAt { get; set; }
        IEventInitiatedViaDTO InitiatedVia { get; set; }
        IMessageCreateDTO MessageCreate { get; set; }
    }
}
