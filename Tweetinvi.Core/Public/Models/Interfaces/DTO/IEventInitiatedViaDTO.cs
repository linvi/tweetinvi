namespace Tweetinvi.Models.DTO
{
    public interface IEventInitiatedViaDTO
    {
        long? TweetId { get; }
        long? WelcomeMessageId { get; }
    }
}
