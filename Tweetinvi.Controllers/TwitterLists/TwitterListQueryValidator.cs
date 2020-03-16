using System;
using Tweetinvi.Core.QueryValidators;
using Tweetinvi.Models;

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

            var isIdValid = twitterListIdentifier.Id > 0;
            var isSlugWithUsernameValid = twitterListIdentifier.Slug != null && (twitterListIdentifier.OwnerScreenName != null || twitterListIdentifier.OwnerId > 0);

            if (!isIdValid && !isSlugWithUsernameValid)
            {
                throw new ArgumentException("List identifier(id or slug + userIdentifier) must be specified.");
            }
        }

        public bool IsListIdentifierValid(ITwitterListIdentifier twitterListIdentifier)
        {
            if (twitterListIdentifier == null)
            {
                return false;
            }

            if (twitterListIdentifier.Id != null && twitterListIdentifier.Id > 0)
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