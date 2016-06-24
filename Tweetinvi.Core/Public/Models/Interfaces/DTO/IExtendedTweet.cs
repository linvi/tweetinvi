using Tweetinvi.Models.Entities;

namespace Tweetinvi.Models.DTO
{
    public interface IExtendedTweet
    {
        string Text { get; set; }

        string FullText { get; set; }

        int[] DisplayTextRange { get; set; }
        ITweetEntities LegacyEntities { get; set; }
        ITweetEntities ExtendedEntities { get; set; }
    }
}
