using Tweetinvi.Core.Models;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.QueryGenerators
{
    public class ComputedTweetMode
    {
        private readonly TweetMode? _tweetMode;

        public ComputedTweetMode(ITweetModeParameter parameter, ITwitterRequest request)
        {
            _tweetMode = parameter?.TweetMode ?? request.ExecutionContext.TweetMode;
        }

        private ComputedTweetMode(TweetMode tweetMode)
        {
            _tweetMode = tweetMode;
        }

        public static implicit operator TweetMode?(ComputedTweetMode computedTweetMode)
        {
            return computedTweetMode._tweetMode;
        }

        public override string ToString()
        {
            if (_tweetMode == TweetMode.None)
            {
                return null;
            }

            return _tweetMode?.ToString().ToLowerInvariant();
        }

        public static ComputedTweetMode Extended => new ComputedTweetMode(TweetMode.Extended);
    }
}