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
                throw new ArgumentNullException("List identifier cannot be null.");
            }

            if (twitterListIdentifier.Id == TweetinviSettings.DEFAULT_ID)
            {
                throw new ArgumentException("List id must be set.");
            }
        }

        public void ThrowIfListUpdateParametersIsNotValid(ITwitterListUpdateParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("List update parameter cannot be null.");
            }
        }

        public bool IsListIdentifierValid(ITwitterListIdentifier twitterListIdentifier)
        {
            if (twitterListIdentifier == null)
            {
                return false;
            }

            if (twitterListIdentifier.Id != TweetinviSettings.DEFAULT_ID)
            {
                return true;
            }

            bool isOwnerIdentifierValid = IsOwnerIdValid(twitterListIdentifier.OwnerId) || IsOwnerScreenNameValid(twitterListIdentifier.OwnerScreenName);
            return IsSlugValid(twitterListIdentifier.Slug) && isOwnerIdentifierValid;
        }

        public void ThrowIfGetTweetsFromListQueryParametersIsNotValid(IGetTweetsFromListQueryParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("GetTweetsFromListQueryP Parameter cannot be null");
            }

            var identifier = parameters.TwitterListIdentifier;

            ThrowIfListIdentifierIsNotValid(identifier);
        }

        public bool IsOwnerScreenNameValid(string ownerScreenName)
        {
            return !string.IsNullOrEmpty(ownerScreenName);
        }

        public bool IsOwnerIdValid(long? ownderId)
        {
            return ownderId != null && ownderId != 0;
        }

        public bool IsSlugValid(string slug)
        {
            return !string.IsNullOrEmpty(slug);
        }
    }
}