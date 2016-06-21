using Tweetinvi.Core.Interfaces.Models.Entities;

namespace Tweetinvi.Core.Interfaces.DTO
{
    public interface IExtendedTweet
    {
        string Text { get; set; }

        string FullText { get; set; }

        int[] DisplayTextRange { get; set; }
        ITweetEntities Entities { get; set; }
    }
}
