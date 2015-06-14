using Tweetinvi.Core.Interfaces.DTO;

namespace Tweetinvi.Controllers.Tweet
{
    public interface ITweetQueryValidator
    {
        bool CanTweetDTOBePublished(ITweetDTO tweetDTO);
        bool CanTweetDTOBeDestroyed(ITweetDTO tweetDTO);
        bool IsTweetPublished(ITweetDTO tweetDTO);
    }

    public class TweetQueryValidator : ITweetQueryValidator
    {
        public bool CanTweetDTOBePublished(ITweetDTO tweet)
        {
            if (tweet == null)
            {
                return false;
            }

            var tooManyMedia = tweet.MediasToPublish != null && tweet.MediasToPublish.Count > 4;
            return !tweet.IsTweetPublished && !tweet.IsTweetDestroyed && !tooManyMedia;
        }

        public bool CanTweetDTOBeDestroyed(ITweetDTO tweet)
        {
            return tweet != null && tweet.IsTweetPublished && !tweet.IsTweetDestroyed;
        }

        public bool IsTweetPublished(ITweetDTO tweet)
        {
            return tweet != null && tweet.IsTweetPublished && !tweet.IsTweetDestroyed;
        }
    }
}