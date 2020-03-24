namespace Tweetinvi.Models.DTO
{
    public interface IMessageCreateDTO
    {
        // Twitter fields
        IMessageCreateTargetDTO Target { get; set; }
        long SenderId { get; set; }
        long? SourceAppId { get; set; }
        IMessageDataDTO MessageData { get; set; }
    }
}
