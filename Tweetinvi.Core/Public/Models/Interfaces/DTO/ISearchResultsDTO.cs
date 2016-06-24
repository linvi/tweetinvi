namespace Tweetinvi.Models.DTO
{
    public interface ISearchResultsDTO
    {
        ITweetWithSearchMetadataDTO[] TweetDTOs { get; set; }
        ITweetWithSearchMetadataDTO[] MatchingTweetDTOs { get; set; }
        ISearchMetadata SearchMetadata { get; set; }
    }
}
