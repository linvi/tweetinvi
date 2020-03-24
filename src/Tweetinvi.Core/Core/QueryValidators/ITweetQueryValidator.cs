using Tweetinvi.Models;

namespace Tweetinvi.Core.QueryValidators
{
    public interface ITweetQueryValidator
    {
        void ThrowIfTweetCannotBeUsed(ITweetIdentifier tweet);
        void ThrowIfTweetCannotBeUsed(ITweetIdentifier tweet, string parameterName);
    }
}