using System;
using Tweetinvi.Core;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.QueryValidators;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Controllers.TwitterLists
{
    public class TwitterListQueryValidator : ITwitterListQueryValidator
    {
        public bool IsListUpdateParametersValid(ITwitterListUpdateParameters parameters)
        {
            return parameters != null;
        }

        public bool IsDescriptionParameterValid(string description)
        {
            return !String.IsNullOrEmpty(description);
        }

        public bool IsNameParameterValid(string name)
        {
            return !String.IsNullOrEmpty(name);
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

        public bool IsOwnerScreenNameValid(string ownerScreenName)
        {
            return !String.IsNullOrEmpty(ownerScreenName);
        }

        public bool IsOwnerIdValid(long? ownderId)
        {
            return ownderId != null && ownderId != 0;
        }

        public bool IsSlugValid(string slug)
        {
            return !String.IsNullOrEmpty(slug);
        }
    }
}