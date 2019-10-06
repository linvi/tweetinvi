using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.QueryValidators
{
    public interface ITweetQueryValidator
    {
        void ThrowIfTweetCannotBePublished(IPublishTweetParameters parameters);
        bool IsTweetPublished(ITweetDTO tweetDTO);
        bool IsValidTweetIdentifier(ITweetIdentifier tweetIdentifier);
        void ThrowIfTweetCannotBeDestroyed(ITweetDTO tweet);
        void ThrowIfTweetCannotBeUsed(ITweetDTO tweet);
        void ThrowIfTweetCannotBeUsed(ITweetIdentifier tweet);
        void ThrowIfTweetCannotBeUsed(long? tweetId);
    }
}