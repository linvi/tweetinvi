using Tweetinvi.Core.DTO;

namespace Tweetinvi.Models.DTO
{
    public interface ISearchResultsDTO
    {
        TweetWithSearchMetadataDTO[] TweetDTOs { get; set; }
        ISearchMetadata SearchMetadata { get; set; }
    }
}
