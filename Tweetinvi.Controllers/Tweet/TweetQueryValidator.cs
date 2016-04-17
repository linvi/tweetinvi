using System;
using System.Linq;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Controllers.Tweet
{
    public interface ITweetQueryValidator
    {
        void ThrowIfTweetCannotBePublished(IPublishTweetParameters parameters);
        bool CanTweetDTOBeDestroyed(ITweetDTO tweetDTO);
        bool IsTweetPublished(ITweetDTO tweetDTO);
        bool IsValidTweetIdentifier(ITweetIdentifier tweetIdentifier);
        void ValidateTweetIdentifier(ITweetIdentifier tweetIdentifier);
    }

    public class TweetQueryValidator : ITweetQueryValidator
    {
        public void ThrowIfTweetCannotBePublished(IPublishTweetParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentException("Publish parameters cannot be null.");
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

        public bool IsValidTweetIdentifier(ITweetIdentifier tweetIdentifier)
        {
            return tweetIdentifier != null && tweetIdentifier.Id > 0;
        }

        public void ValidateTweetIdentifier(ITweetIdentifier tweetIdentifier)
        {
            if (tweetIdentifier == null)
            {
                throw new ArgumentException("TweetIdentifier");
            }

            if (!IsValidTweetIdentifier(tweetIdentifier))
            {
                throw new ArgumentException("TweetIdentifier is not valid.");
            }
        }
    }
}