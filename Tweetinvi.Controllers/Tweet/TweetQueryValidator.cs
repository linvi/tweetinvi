using System;
using System.Linq;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.Parameters.QueryParameters;

namespace Tweetinvi.Controllers.Tweet
{
    public interface ITweetQueryValidator
    {
        void ThrowIfTweetCannotBePublished(IPublishTweetParameters parameters);
        bool CanTweetDTOBeDestroyed(ITweetDTO tweetDTO);
        bool IsTweetPublished(ITweetDTO tweetDTO);
    }

    public class TweetQueryValidator : ITweetQueryValidator
    {
        public void ThrowIfTweetCannotBePublished(IPublishTweetParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("Publish parameters cannot be null.");
            }

            if (string.IsNullOrEmpty(parameters.Text))
            {
                throw new ArgumentException("The text message of a tweet cannot be null or empty.");
            }

            var mediaObjectIds = parameters.Medias.Where(x => x.MediaId != null).Select(x => x.MediaId.Value);
            var mediaIds = parameters.MediaIds.Concat(mediaObjectIds).Distinct();

            if (mediaIds.Count() > 4)
            {
                throw new ArgumentException("Cannot publish a tweet with more than 4 medias.");
            }
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