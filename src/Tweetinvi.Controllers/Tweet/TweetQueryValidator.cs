using System;
using System.Linq;
using Tweetinvi.Core.QueryValidators;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Tweet
{
    public class TweetQueryValidator : ITweetQueryValidator
    {
        public void ThrowIfTweetCannotBePublished(IPublishTweetParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentException("Publish parameters cannot be null.");
            }

            if (string.IsNullOrEmpty(parameters.Text) && !parameters.HasMedia)
            {
                throw new ArgumentException("The text message of a tweet cannot be null or empty, unless media is attached.");
            }

            var mediaObjectIds = parameters.Medias.Where(x => x.Id != null).Select(x => x.Id.Value);
            var mediaIds = parameters.MediaIds.Concat(mediaObjectIds).Distinct();

            if (mediaIds.Count() > 4)
            {
                throw new ArgumentException("Cannot publish a tweet with more than 4 medias.");
            }
        }

        public void ThrowIfTweetCannotBeUsed(ITweetIdentifier tweet)
        {
            ThrowIfTweetCannotBeUsed(tweet, $"{nameof(tweet)}.{nameof(tweet.Id)}");
        }

        public void ThrowIfTweetCannotBeUsed(ITweetIdentifier tweet, string parameterName)
        {
            if (tweet == null)
            {
                throw new ArgumentNullException($"{nameof(tweet)}");
            }

            if (!IsValidTweetIdentifier(tweet))
            {
                throw new ArgumentException(parameterName);
            }
        }

        private bool IsValidTweetIdentifier(ITweetIdentifier tweetIdentifier)
        {
            return tweetIdentifier != null && tweetIdentifier.Id > 0;
        }
    }
}