namespace Tweetinvi.Core.Interfaces.DTO
{
    public interface ITweetWithSearchMetadataDTO : ITweetDTO
    {
        ITweetFromSearchMetadata TweetFromSearchMetadata { get; }
    }
}