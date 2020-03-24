namespace Tweetinvi.Models.DTO
{
    public interface ITweetWithSearchMetadataDTO : ITweetDTO
    {
        ITweetFromSearchMetadata TweetFromSearchMetadata { get; }
    }
}