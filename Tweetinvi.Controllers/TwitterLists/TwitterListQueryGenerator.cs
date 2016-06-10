using System.Collections.Generic;
using System.Text;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Controllers.Shared;
using Tweetinvi.Core;
using Tweetinvi.Core.Extensions;
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
            _userQueryValidator.ThrowIfUserCannotBeIdentified(userIdentifier);

            var identifierParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(userIdentifier);
            return string.Format(Resources.List_GetUserLists, identifierParameter, getOwnedListsFirst);
        }

        // Owned Lists
        public string GetUsersOwnedListQuery(IUserIdentifier userIdentifier, int maximumNumberOfListsToRetrieve)
        {
            _userQueryValidator.ThrowIfUserCannotBeIdentified(userIdentifier);

            var identifierParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(userIdentifier);
            return string.Format(Resources.List_Ownership, identifierParameter, maximumNumberOfListsToRetrieve);
        }

        // Update
        public string GetUpdateListQuery(ITwitterListUpdateQueryParameters parameters)
        {
            _listsQueryValidator.ThrowIfListIdentifierIsNotValid(parameters.TwitterListIdentifier);
            _listsQueryValidator.ThrowIfListUpdateParametersIsNotValid(parameters.Parameters);

            var listIdentifierParameter = _twitterListQueryParameterGenerator.GenerateIdentifierParameter(parameters.TwitterListIdentifier);
            var updateQueryParameters = GenerateUpdateAdditionalParameters(parameters.Parameters);

            var queryParameters = string.Format("{0}{1}", listIdentifierParameter, updateQueryParameters);
            return string.Format(Resources.List_Update, queryParameters);
        }
        
        private string GenerateUpdateAdditionalParameters(ITwitterListUpdateParameters parameters)
        {
            var privacyModeParameter = string.Format(Resources.List_PrivacyModeParameter, parameters.PrivacyMode.ToString().ToLower());

            var queryParameterBuilder = new StringBuilder(privacyModeParameter);

            queryParameterBuilder.AddParameterToQuery("description", parameters.Description);
            queryParameterBuilder.AddParameterToQuery("name", parameters.Name);

            return queryParameterBuilder.ToString();
        }
        
        public string GetDestroyListQuery(ITwitterListIdentifier identifier)
        {
            _listsQueryValidator.ThrowIfListIdentifierIsNotValid(identifier);

            var identifierParameter = _twitterListQueryParameterGenerator.GenerateIdentifierParameter(identifier);
            return string.Format(Resources.List_Destroy, identifierParameter);
        }

        public string GetTweetsFromListQuery(IGetTweetsFromListQueryParameters getTweetsFromListQueryParameters)
        {
            _listsQueryValidator.ThrowIfGetTweetsFromListQueryParametersIsNotValid(getTweetsFromListQueryParameters);

            var identifier = getTweetsFromListQueryParameters.TwitterListIdentifier;
            var parameters = getTweetsFromListQueryParameters.Parameters;

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
            _listsQueryValidator.ThrowIfListIdentifierIsNotValid(listIdentifier);

            var identifierParameter = _twitterListQueryParameterGenerator.GenerateIdentifierParameter(listIdentifier);
            return string.Format(Resources.List_Members, identifierParameter, maximumNumberOfMembers);
        }

        public string GetAddMemberToListQuery(ITwitterListIdentifier listIdentifier, IUserIdentifier userIdentifier)
        {
            _userQueryValidator.ThrowIfUserCannotBeIdentified(userIdentifier);
            _listsQueryValidator.ThrowIfListIdentifierIsNotValid(listIdentifier);

            var listIdentifierParameter = _twitterListQueryParameterGenerator.GenerateIdentifierParameter(listIdentifier);
            var userIdentifierParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(userIdentifier);

            return string.Format(Resources.List_CreateMember, listIdentifierParameter, userIdentifierParameter);
        }

        public string GetAddMultipleMembersToListQuery(ITwitterListIdentifier listIdentifier, IEnumerable<IUserIdentifier> userIdentifiers)
        {
            _listsQueryValidator.ThrowIfListIdentifierIsNotValid(listIdentifier);

            var userIdsAndScreenNameParameter = _userQueryParameterGenerator.GenerateListOfUserIdentifiersParameter(userIdentifiers);
            var query = new StringBuilder(Resources.List_CreateMembers);

            query.AddFormattedParameterToQuery(_twitterListQueryParameterGenerator.GenerateIdentifierParameter(listIdentifier));
            query.AddFormattedParameterToQuery(userIdsAndScreenNameParameter);

            return query.ToString();
        }

        public string GetRemoveMemberFromListQuery(ITwitterListIdentifier listIdentifier, IUserIdentifier userIdentifier)
        {
            _listsQueryValidator.ThrowIfListIdentifierIsNotValid(listIdentifier);
            _userQueryValidator.ThrowIfUserCannotBeIdentified(userIdentifier);

            var listIdentifierParameter = _twitterListQueryParameterGenerator.GenerateIdentifierParameter(listIdentifier);
            var userIdentifierParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(userIdentifier);

            return string.Format(Resources.List_DestroyMember, listIdentifierParameter, userIdentifierParameter);
        }

        public string GetRemoveMultipleMembersFromListQuery(ITwitterListIdentifier listIdentifier, IEnumerable<IUserIdentifier> userIdentifiers)
        {
            _listsQueryValidator.ThrowIfListIdentifierIsNotValid(listIdentifier);

            var listIdentifierParameter = _twitterListQueryParameterGenerator.GenerateIdentifierParameter(listIdentifier);
            var userIdsAndScreenNameParameter = _userQueryParameterGenerator.GenerateListOfUserIdentifiersParameter(userIdentifiers);

            var query = new StringBuilder(Resources.List_DestroyMembers);

            query.AddFormattedParameterToQuery(listIdentifierParameter);
            query.AddFormattedParameterToQuery(userIdsAndScreenNameParameter);

            return query.ToString();
        }

        public string GetCheckIfUserIsAListMemberQuery(ITwitterListIdentifier listIdentifier, IUserIdentifier userIdentifier)
        {
            _listsQueryValidator.ThrowIfListIdentifierIsNotValid(listIdentifier);
            _userQueryValidator.ThrowIfUserCannotBeIdentified(userIdentifier);

            var listIdentifierParameter = _twitterListQueryParameterGenerator.GenerateIdentifierParameter(listIdentifier);
            var userIdentifierParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(userIdentifier);

            return string.Format(Resources.List_CheckMembership, listIdentifierParameter, userIdentifierParameter);
        }

        // Subscriptions
        public string GetUserSubscribedListsQuery(IUserIdentifier userIdentifier, int maximumNumberOfListsToRetrieve)
        {
            _userQueryValidator.ThrowIfUserCannotBeIdentified(userIdentifier);

            var userIdParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(userIdentifier);
            return string.Format(Resources.List_UserSubscriptions, userIdParameter, maximumNumberOfListsToRetrieve);
        }

        public string GetListSubscribersQuery(ITwitterListIdentifier listIdentifier, int maximumNumberOfSubscribersToRetrieve)
        {
            _listsQueryValidator.ThrowIfListIdentifierIsNotValid(listIdentifier);

            var identifierParameter = _twitterListQueryParameterGenerator.GenerateIdentifierParameter(listIdentifier);
            return string.Format(Resources.List_GetSubscribers, identifierParameter, maximumNumberOfSubscribersToRetrieve);
        }

        public string GetSubscribeUserToListQuery(ITwitterListIdentifier listIdentifier)
        {
            _listsQueryValidator.ThrowIfListIdentifierIsNotValid(listIdentifier);
            
            var listIdentifierParameter = _twitterListQueryParameterGenerator.GenerateIdentifierParameter(listIdentifier);
            return string.Format(Resources.List_Subscribe, listIdentifierParameter);
        }

        public string GetUnSubscribeUserFromListQuery(ITwitterListIdentifier listIdentifier)
        {
            _listsQueryValidator.ThrowIfListIdentifierIsNotValid(listIdentifier);

            var listIdentifierParameter = _twitterListQueryParameterGenerator.GenerateIdentifierParameter(listIdentifier);
            return string.Format(Resources.List_UnSubscribe, listIdentifierParameter);
        }

        public string GetCheckIfUserIsAListSubscriberQuery(ITwitterListIdentifier listIdentifier, IUserIdentifier userIdentifier)
        {
            _listsQueryValidator.ThrowIfListIdentifierIsNotValid(listIdentifier);
            _userQueryValidator.ThrowIfUserCannotBeIdentified(userIdentifier);

            var listIdentifierParameter = _twitterListQueryParameterGenerator.GenerateIdentifierParameter(listIdentifier);
            var userIdentifierParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(userIdentifier);

            var query = new StringBuilder(Resources.List_CheckSubscriber);

            query.AddFormattedParameterToQuery(listIdentifierParameter);
            query.AddFormattedParameterToQuery(userIdentifierParameter);
            query.AddParameterToQuery("skip_status", "true");

            return query.ToString();
        }
    }
}