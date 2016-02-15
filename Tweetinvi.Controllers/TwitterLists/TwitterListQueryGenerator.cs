using System;
using System.Collections.Generic;
using System.Text;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Controllers.Shared;
using Tweetinvi.Core;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.QueryGenerators;
using Tweetinvi.Core.Interfaces.QueryValidators;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.Parameters.QueryParameters;

namespace Tweetinvi.Controllers.TwitterLists
{
    public interface ITwitterListQueryGenerator
    {
        string GetUserSubscribedListsQuery(IUserIdentifier userIdentifier, bool getOwnedListsFirst);
        string GetUserSubscribedListsQuery(long userId, bool getOwnedListsFirst);
        string GetUserSubscribedListsQuery(string userScreenName, bool getOwnedListsFirst);

        string GetUsersOwnedListQuery(IUserIdentifier userIdentifier, int maximumNumberOfListsToRetrieve);

        string GetUpdateListQuery(ITwitterListUpdateQueryParameters parameters);
        string GetDestroyListQuery(ITwitterListIdentifier identifier);
        string GetTweetsFromListQuery(IGetTweetsFromListQueryParameters queryParameters);
        
        string GetMembersFromListQuery(ITwitterListIdentifier listIdentifier, int maximumNumberOfMembers);
        string GetAddMemberToListQuery(ITwitterListIdentifier listIdentifier, IUserIdentifier userIdentifier);
        string GetAddMultipleMembersToListQuery(ITwitterListIdentifier listIdentifier, IEnumerable<IUserIdentifier> userIdentifiers);
        string GetRemoveMemberFromListQuery(ITwitterListIdentifier listIdentifier, IUserIdentifier userIdentifier);
        string GetRemoveMultipleMembersFromListQuery(ITwitterListIdentifier listIdentifier, IEnumerable<IUserIdentifier> userIdentifiers);
        string GetCheckIfUserIsAListMemberQuery(ITwitterListIdentifier listIdentifier, IUserIdentifier userIdentifier);

        string GetUserSubscribedListsQuery(IUserIdentifier userIdentifier, int maximumNumberOfListsToRetrieve);

        string GetListSubscribersQuery(ITwitterListIdentifier listIdentifier, int maximumNumberOfSubscribersToRetrieve);
        string GetSubscribeUserToListQuery(ITwitterListIdentifier listIdentifier);
        string GetUnSubscribeUserFromListQuery(ITwitterListIdentifier listIdentifier);
        string GetCheckIfUserIsAListSubscriberQuery(ITwitterListIdentifier listIdentifier, IUserIdentifier userIdentifier);

    }

    public class TwitterListQueryGenerator : ITwitterListQueryGenerator
    {
        private readonly ITwitterListQueryValidator _listsQueryValidator;
        private readonly IUserQueryParameterGenerator _userQueryParameterGenerator;
        private readonly IUserQueryValidator _userQueryValidator;
        private readonly IQueryParameterGenerator _queryParameterGenerator;
        private readonly ITwitterListQueryParameterGenerator _twitterListQueryParameterGenerator;

        public TwitterListQueryGenerator(
            ITwitterListQueryValidator listsQueryValidator,
            IUserQueryParameterGenerator userQueryParameterGenerator,
            IUserQueryValidator userQueryValidator,
            IQueryParameterGenerator queryParameterGenerator,
            ITwitterListQueryParameterGenerator twitterListQueryParameterGenerator)
        {
            _listsQueryValidator = listsQueryValidator;
            _userQueryParameterGenerator = userQueryParameterGenerator;
            _userQueryValidator = userQueryValidator;
            _queryParameterGenerator = queryParameterGenerator;
            _twitterListQueryParameterGenerator = twitterListQueryParameterGenerator;
        }

        // User Lists
        public string GetUserSubscribedListsQuery(IUserIdentifier userIdentifier, bool getOwnedListsFirst)
        {
            if (!_userQueryValidator.CanUserBeIdentified(userIdentifier))
            {
                return null;
            }

            var userIdParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(userIdentifier);
            return GenerateUserListsQuery(userIdParameter, getOwnedListsFirst);
        }

        public string GetUserSubscribedListsQuery(long userId, bool getOwnedListsFirst)
        {
            if (!_userQueryValidator.IsUserIdValid(userId))
            {
                return null;
            }

            var userIdentifier = _userQueryParameterGenerator.GenerateUserIdParameter(userId);
            return GenerateUserListsQuery(userIdentifier, getOwnedListsFirst);
        }

        public string GetUserSubscribedListsQuery(string userScreenName, bool getOwnedListsFirst)
        {
            if (!_userQueryValidator.IsScreenNameValid(userScreenName))
            {
                return null;
            }

            var userIdentifier = _userQueryParameterGenerator.GenerateScreenNameParameter(userScreenName);
            return GenerateUserListsQuery(userIdentifier, getOwnedListsFirst);
        }

        private string GenerateUserListsQuery(string userIdentifier, bool getOwnedListsFirst)
        {
            return string.Format(Resources.List_GetUserLists, userIdentifier, getOwnedListsFirst);
        }

        // Owned Lists
        public string GetUsersOwnedListQuery(IUserIdentifier userIdentifier, int maximumNumberOfListsToRetrieve)
        {
            if (!_userQueryValidator.CanUserBeIdentified(userIdentifier))
            {
                return null;
            }

            var identifierParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(userIdentifier);
            return string.Format(Resources.List_Ownership, identifierParameter, maximumNumberOfListsToRetrieve);
        }

        // Update
        public string GetUpdateListQuery(ITwitterListUpdateQueryParameters parameters)
        {
            if (!_listsQueryValidator.IsListIdentifierValid(parameters.TwitterListIdentifier) || 
                !_listsQueryValidator.IsListUpdateParametersValid(parameters.Parameters))
            {
                return null;
            }

            var listIdentifierParameter = _twitterListQueryParameterGenerator.GenerateIdentifierParameter(parameters.TwitterListIdentifier);
            var updateQueryParameters = GenerateUpdateAdditionalParameters(parameters.Parameters);

            var queryParameters = string.Format("{0}{1}", listIdentifierParameter, updateQueryParameters);
            return string.Format(Resources.List_Update, queryParameters);
        }

        
        private string GenerateUpdateAdditionalParameters(ITwitterListUpdateParameters parameters)
        {
            string privacyModeParameter = string.Format(Resources.List_PrivacyModeParameter, parameters.PrivacyMode.ToString().ToLower());

            StringBuilder queryParameterBuilder = new StringBuilder(privacyModeParameter);

            if (_listsQueryValidator.IsDescriptionParameterValid(parameters.Description))
            {
                string descriptionParameter = string.Format(Resources.List_DescriptionParameter, parameters.Description);
                queryParameterBuilder.Append(descriptionParameter);
            }

            if (_listsQueryValidator.IsNameParameterValid(parameters.Name))
            {
                string nameParameter = string.Format(Resources.List_NameParameter, parameters.Name);
                queryParameterBuilder.Append(nameParameter);
            }

            return queryParameterBuilder.ToString();
        }

        
        public string GetDestroyListQuery(ITwitterListIdentifier identifier)
        {
            if (!_listsQueryValidator.IsListIdentifierValid(identifier))
            {
                return null;
            }

            var identifierParameter = _twitterListQueryParameterGenerator.GenerateIdentifierParameter(identifier);
            return string.Format(Resources.List_Destroy, identifierParameter);
        }
        

        public string GetTweetsFromListQuery(IGetTweetsFromListQueryParameters getTweetsFromListQueryParameters)
        {
            var identifier = getTweetsFromListQueryParameters.TwitterListIdentifier;
            var parameters = getTweetsFromListQueryParameters.Parameters;

            if (!_listsQueryValidator.IsListIdentifierValid(identifier))
            {
                return null;
            }

            StringBuilder queryParameters = new StringBuilder();

            queryParameters.Append(_twitterListQueryParameterGenerator.GenerateIdentifierParameter(identifier));

            if (parameters != null)
            {
                queryParameters.Append(_queryParameterGenerator.GenerateSinceIdParameter(parameters.SinceId));
                queryParameters.Append(_queryParameterGenerator.GenerateMaxIdParameter(parameters.MaxId));
                queryParameters.Append(_queryParameterGenerator.GenerateCountParameter(parameters.MaximumNumberOfTweetsToRetrieve));
                queryParameters.Append(_queryParameterGenerator.GenerateIncludeEntitiesParameter(parameters.IncludeEntities));
                queryParameters.Append(_queryParameterGenerator.GenerateIncludeRetweetsParameter(parameters.IncludeRetweets));
            }
            else
            {
                queryParameters.Append(_queryParameterGenerator.GenerateCountParameter(TweetinviConsts.LIST_GET_TWEETS_COUNT));
            }

            return string.Format(Resources.List_GetTweetsFromList, queryParameters);
        }

        // Members
        public string GetMembersFromListQuery(ITwitterListIdentifier listIdentifier, int maximumNumberOfMembers)
        {
            if (!_listsQueryValidator.IsListIdentifierValid(listIdentifier))
            {
                return null;
            }

            var identifierParameter = _twitterListQueryParameterGenerator.GenerateIdentifierParameter(listIdentifier);
            return string.Format(Resources.List_Members, identifierParameter, maximumNumberOfMembers);
        }

        public string GetAddMemberToListQuery(ITwitterListIdentifier listIdentifier, IUserIdentifier userIdentifier)
        {
            if (!_listsQueryValidator.IsListIdentifierValid(listIdentifier) ||
                !_userQueryValidator.CanUserBeIdentified(userIdentifier))
            {
                return null;
            }

            var listIdentifierParameter = _twitterListQueryParameterGenerator.GenerateIdentifierParameter(listIdentifier);
            var userIdentifierParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(userIdentifier);

            return string.Format(Resources.List_CreateMember, listIdentifierParameter, userIdentifierParameter);
        }

        public string GetAddMultipleMembersToListQuery(ITwitterListIdentifier listIdentifier, IEnumerable<IUserIdentifier> userIdentifiers)
        {
            if (userIdentifiers == null)
            {
                return null;
            }

            string userIdsAndScreenNameParameter = _userQueryParameterGenerator.GenerateListOfUserIdentifiersParameter(userIdentifiers);
            return string.Format("https://api.twitter.com/1.1/lists/members/create_all.json?{0}", userIdsAndScreenNameParameter);
        }

        public string GetRemoveMemberFromListQuery(ITwitterListIdentifier listIdentifier, IUserIdentifier userIdentifier)
        {
            if (!_listsQueryValidator.IsListIdentifierValid(listIdentifier) ||
                !_userQueryValidator.CanUserBeIdentified(userIdentifier))
            {
                return null;
            }

            var listIdentifierParameter = _twitterListQueryParameterGenerator.GenerateIdentifierParameter(listIdentifier);
            var userIdentifierParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(userIdentifier);

            return string.Format(Resources.List_DestroyMember, listIdentifierParameter, userIdentifierParameter);
        }

        public string GetRemoveMultipleMembersFromListQuery(ITwitterListIdentifier listIdentifier, IEnumerable<IUserIdentifier> userIdentifiers)
        {
            if (userIdentifiers == null)
            {
                return null;
            }

            string userIdsAndScreenNameParameter = _userQueryParameterGenerator.GenerateListOfUserIdentifiersParameter(userIdentifiers);
            return string.Format("https://api.twitter.com/1.1/lists/members/destroy_all.json?{0}", userIdsAndScreenNameParameter);
        }

        public string GetCheckIfUserIsAListMemberQuery(ITwitterListIdentifier listIdentifier, IUserIdentifier userIdentifier)
        {
            if (!_listsQueryValidator.IsListIdentifierValid(listIdentifier) ||
                !_userQueryValidator.CanUserBeIdentified(userIdentifier))
            {
                return null;
            }

            var listIdentifierParameter = _twitterListQueryParameterGenerator.GenerateIdentifierParameter(listIdentifier);
            var userIdentifierParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(userIdentifier);

            return string.Format(Resources.List_CheckMembership, listIdentifierParameter, userIdentifierParameter);
        }

        // Subscriptions
        public string GetUserSubscribedListsQuery(IUserIdentifier userIdentifier, int maximumNumberOfListsToRetrieve)
        {
            if (!_userQueryValidator.CanUserBeIdentified(userIdentifier))
            {
                return null;
            }

            var userIdParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(userIdentifier);
            return string.Format(Resources.List_UserSubscriptions, userIdParameter, maximumNumberOfListsToRetrieve);
        }

        public string GetListSubscribersQuery(ITwitterListIdentifier listIdentifier, int maximumNumberOfSubscribersToRetrieve)
        {
            if (!_listsQueryValidator.IsListIdentifierValid(listIdentifier))
            {
                return null;
            }

            var identifierParameter = _twitterListQueryParameterGenerator.GenerateIdentifierParameter(listIdentifier);
            return string.Format(Resources.List_GetSubscribers, identifierParameter, maximumNumberOfSubscribersToRetrieve);
        }

        public string GetSubscribeUserToListQuery(ITwitterListIdentifier listIdentifier)
        {
            if (!_listsQueryValidator.IsListIdentifierValid(listIdentifier))
            {
                return null;
            }

            var listIdentifierParameter = _twitterListQueryParameterGenerator.GenerateIdentifierParameter(listIdentifier);

            return string.Format(Resources.List_Subscribe, listIdentifierParameter);
        }

        public string GetUnSubscribeUserFromListQuery(ITwitterListIdentifier listIdentifier)
        {
            if (!_listsQueryValidator.IsListIdentifierValid(listIdentifier))
            {
                return null;
            }

            var listIdentifierParameter = _twitterListQueryParameterGenerator.GenerateIdentifierParameter(listIdentifier);

            return string.Format(Resources.List_UnSubscribe, listIdentifierParameter);
        }

        public string GetCheckIfUserIsAListSubscriberQuery(ITwitterListIdentifier listIdentifier, IUserIdentifier userIdentifier)
        {
            if (!_listsQueryValidator.IsListIdentifierValid(listIdentifier) ||
                !_userQueryValidator.CanUserBeIdentified(userIdentifier))
            {
                return null;
            }

            var listIdentifierParameter = _twitterListQueryParameterGenerator.GenerateIdentifierParameter(listIdentifier);
            var userIdentifierParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(userIdentifier);

            return string.Format("https://api.twitter.com/1.1/lists/subscribers/show.json?{0}&{1}&skip_status=true", listIdentifierParameter, userIdentifierParameter);
        }
    }
}