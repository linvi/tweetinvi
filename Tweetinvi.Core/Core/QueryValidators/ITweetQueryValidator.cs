using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.QueryValidators
{
    public interface ITweetQueryValidator
    {
        void ThrowIfTweetCannotBeUsed(ITweetIdentifier tweet);
        void ThrowIfTweetCannotBeUsed(ITweetIdentifier tweet, string parameterName);
    }
}