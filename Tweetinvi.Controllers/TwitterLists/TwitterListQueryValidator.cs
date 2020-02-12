using System;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.QueryValidators;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.TwitterLists
{
    public class TwitterListQueryValidator : ITwitterListQueryValidator
    {
        public void ThrowIfListIdentifierIsNotValid(ITwitterListIdentifier twitterListIdentifier)
        {
            if (twitterListIdentifier == null)
            {
                throw new ArgumentNullException(nameof(twitterListIdentifier), $"{nameof(twitterListIdentifier)} cannot be null");
            }

            var isIdValid = twitterListIdentifier.Id != TweetinviSettings.DEFAULT_ID;
            var isSlugWithUsernameValid = twitterListIdentifier.Slug != null && (twitterListIdentifier.OwnerScreenName != null || twitterListIdentifier.OwnerId != TweetinviSettings.DEFAULT_ID);

            if (!isIdValid && !isSlugWithUsernameValid)
            {
                throw new ArgumentException("List identifier(id or slug + userIdentifier) must be specified.");
            }
        }

        public void ThrowIfListUpdateParametersIsNotValid(ITwitterListUpdateParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters), $"{nameof(parameters)} cannot be null");
            }
        }

        public bool IsListIdentifierValid(ITwitterListIdentifier twitterListIdentifier)
        {
            if (twitterListIdentifier == null)
            {
                return false;
            }

            if (twitterListIdentifier.Id != null && twitterListIdentifier.Id != TweetinviSettings.DEFAULT_ID)
            {
                return true;
            }

            var isOwnerIdentifierValid = IsOwnerIdValid(twitterListIdentifier.OwnerId) || IsOwnerScreenNameValid(twitterListIdentifier.OwnerScreenName);
            return IsSlugValid(twitterListIdentifier.Slug) && isOwnerIdentifierValid;
        }

        private static bool IsOwnerScreenNameValid(string ownerScreenName)
        {
            return !string.IsNullOrEmpty(ownerScreenName);
        }

        private static bool IsOwnerIdValid(long? ownerId)
        {
            return ownerId != null && ownerId != 0;
        }

        private static bool IsSlugValid(string slug)
        {
            return !string.IsNullOrEmpty(slug);
        }
    }
}