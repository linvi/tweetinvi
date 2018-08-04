namespace Tweetinvi.Models.DTO
{
    /// <summary>
    /// DTO for both the request and response when creating a message
    /// https://developer.twitter.com/en/docs/direct-messages/sending-and-receiving/api-reference/new-event
    /// </summary>
    public interface ICreateMessageDTO
    {
        IEventDTO Event { get; set; }

        // Note the lack of the App field.
        // The client would need to fill this themselves on the Message if required
    }
}
