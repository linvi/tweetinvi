using System.Collections.Generic;
using System.Text;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Controllers.Shared;
using Tweetinvi.Core;
using Tweetinvi.Core.Core.Parameters;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.QueryValidators;
using Tweetinvi.Models;

namespace Tweetinvi.Controllers.TwitterLists
{
    public interface ITwitterListQueryGenerator
    {
        string GetUserSubscribedListsQuery(IUserIdentifier user, bool getOwnedListsFirst);

        string GetUsersOwnedListQuery(IUserIdentifier user, int maximumNumberOfListsToRetrieve);

        string GetUpdateListQuery(ITwitterListUpdateQueryParameters parameters);
        string GetDestroyListQuery(ITwitterListIdentifier identifier);
        string GetTweetsFromListQuery(IGetTweetsFromListQueryParameters queryParameters);

        string GetMembersFromListQuery(ITwitterListIdentifier listIdentifier, int maximumNumberOfMembers);
        string GetAddMemberToListQuery(ITwitterListIdentifier listIdentifier, IUserIdentifier user);
        string GetAddMultipleMembersToListQuery(ITwitterListIdentifier listIdentifier, IEnumerable<IUserIdentifier> users);
        string GetRemoveMemberFromListQuery(ITwitterListIdentifier listIdentifier, IUserIdentifier user);
        string GetRemoveMultipleMembersFromListQuery(ITwitterListIdentifier listIdentifier, IEnumerable<IUserIdentifier> users);
        string GetCheckIfUserIsAListMemberQuery(ITwitterListIdentifier listIdentifier, IUserIdentifier user);

        string GetUserSubscribedListsQuery(IUserIdentifier user, int maximumNumberOfListsToRetrieve);

        string GetListSubscribersQuery(ITwitterListIdentifier listIdentifier, int maximumNumberOfSubscribersToRetrieve);
        string GetSubscribeUserToListQuery(ITwitterListIdentifier listIdentifier);
        string GetUnSubscribeUserFromListQuery(ITwitterListIdentifier listIdentifier);
        string GetCheckIfUserIsAListSubscriberQuery(ITwitterListIdentifier listIdentifier, IUserIdentifier user);
        string GetUserListMembershipsQuery(IGetUserListMembershipsQueryParameters parameters);
    }

    public class TwitterListQueryGenerator : ITwitterListQueryGenerator
    {
        private readonly ITwitterListQueryValidator _listsQueryValidator;
        private readonly IUserQueryParameterGenerator _userQueryParameterGenerator;
        private readonly IUserQueryValidator _userQueryValidator;
        private readonly IQueryParameterGenerator _queryParameterGenerator;
        private readonly ITweetinviSettingsAccessor _tweetinviSettingsAccessor;
        private readonly ITwitterListQueryParameterGenerator _twitterListQueryParameterGenerator;

        public TwitterListQueryGenerator(
            ITwitterListQueryValidator listsQueryValidator,
            IUserQueryParameterGenerator userQueryParameterGenerator,
            IUserQueryValidator userQueryValidator,
            IQueryParameterGenerator queryParameterGenerator,
            ITweetinviSettingsAccessor tweetinviSettingsAccessor,
            ITwitterListQueryParameterGenerator twitterListQueryParameterGenerator)
        {
            _listsQueryValidator = listsQueryValidator;
            _userQueryParameterGenerator = userQueryParameterGenerator;
            _userQueryValidator = userQueryValidator;
            _queryParameterGenerator = queryParameterGenerator;
            _tweetinviSettingsAccessor = tweetinviSettingsAccessor;
            _twitterListQueryParameterGenerator = twitterListQueryParameterGenerator;
        }

        // User Lists
        public string GetUserSubscribedListsQuery(IUserIdentifier user, bool getOwnedListsFirst)
        {
            _userQueryValidator.ThrowIfUserCannotBeIdentified(user);

            var identifierParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(user);
            return string.Format(Resources.List_GetUserLists, identifierParameter, getOwnedListsFirst);
        }

        public string GetUserListMembershipsQuery(IGetUserListMembershipsQueryParameters parameters)
        {
            _userQueryValidator.ThrowIfUserCannotBeIdentified(parameters.UserIdentifier);

            var userIdentifierParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(parameters.UserIdentifier);
            var additionalParameters = parameters.Parameters;

            var baseQuery = string.Format(Resources.List_GetUserMemberships, userIdentifierParameter);
            var queryBuilder = new StringBuilder(baseQuery);

            queryBuilder.AddParameterToQuery("count", additionalParameters.MaximumNumberOfResults);
            queryBuilder.AddParameterToQuery("filter_to_owned_lists", additionalParameters.FilterToOwnLists);

            return queryBuilder.ToString();
        }

        // Owned Lists
        public string GetUsersOwnedListQuery(IUserIdentifier user, int maximumNumberOfListsToRetrieve)
        {
            _userQueryValidator.ThrowIfUserCannotBeIdentified(user);

            var identifierParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(user);
            return string.Format(Resources.List_Ownership, identifierParameter, maximumNumberOfListsToRetrieve);
        }

        // Update
        public string GetUpdateListQuery(ITwitterListUpdateQueryParameters parameters)
        {
            _listsQueryValidator.ThrowIfListIdentifierIsNotValid(parameters.TwitterListIdentifier);
            _listsQueryValidator.ThrowIfListUpdateParametersIsNotValid(parameters.Parameters);

            var queryBuilder = new StringBuilder(Resources.List_Update);

            var listIdentifierParameter = _twitterListQueryParameterGenerator.GenerateIdentifierParameter(parameters.TwitterListIdentifier);
            queryBuilder.AddFormattedParameterToQuery(listIdentifierParameter);

            queryBuilder.AddParameterToQuery("mode", parameters.Parameters.PrivacyMode.ToString().ToLowerInvariant());
            queryBuilder.AddParameterToQuery("description", parameters.Parameters.Description);
            queryBuilder.AddParameterToQuery("name", parameters.Parameters.Name);
            
            return queryBuilder.ToString();
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

            queryParameters.AddFormattedParameterToParametersList(_queryParameterGenerator.GenerateTweetModeParameter(_tweetinviSettingsAccessor.CurrentThreadSettings.TweetMode));

            return string.Format(Resources.List_GetTweetsFromList, queryParameters);
        }

        // Members
        public string GetMembersFromListQuery(ITwitterListIdentifier listIdentifier, int maximumNumberOfMembers)
        {
            _listsQueryValidator.ThrowIfListIdentifierIsNotValid(listIdentifier);

            var identifierParameter = _twitterListQueryParameterGenerator.GenerateIdentifierParameter(listIdentifier);
            return string.Format(Resources.List_Members, identifierParameter, maximumNumberOfMembers);
        }

        public string GetAddMemberToListQuery(ITwitterListIdentifier listIdentifier, IUserIdentifier user)
        {
            _userQueryValidator.ThrowIfUserCannotBeIdentified(user);
            _listsQueryValidator.ThrowIfListIdentifierIsNotValid(listIdentifier);

            var listIdentifierParameter = _twitterListQueryParameterGenerator.GenerateIdentifierParameter(listIdentifier);
            var userParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(user);

            return string.Format(Resources.List_CreateMember, listIdentifierParameter, userParameter);
        }

        public string GetAddMultipleMembersToListQuery(ITwitterListIdentifier listIdentifier, IEnumerable<IUserIdentifier> users)
        {
            _listsQueryValidator.ThrowIfListIdentifierIsNotValid(listIdentifier);

            var userIdsAndScreenNameParameter = _userQueryParameterGenerator.GenerateListOfUserIdentifiersParameter(users);
            var query = new StringBuilder(Resources.List_CreateMembers);

            query.AddFormattedParameterToQuery(_twitterListQueryParameterGenerator.GenerateIdentifierParameter(listIdentifier));
            query.AddFormattedParameterToQuery(userIdsAndScreenNameParameter);

            return query.ToString();
        }

        public string GetRemoveMemberFromListQuery(ITwitterListIdentifier listIdentifier, IUserIdentifier user)
        {
            _listsQueryValidator.ThrowIfListIdentifierIsNotValid(listIdentifier);
            _userQueryValidator.ThrowIfUserCannotBeIdentified(user);

            var listIdentifierParameter = _twitterListQueryParameterGenerator.GenerateIdentifierParameter(listIdentifier);
            var userParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(user);

            return string.Format(Resources.List_DestroyMember, listIdentifierParameter, userParameter);
        }

        public string GetRemoveMultipleMembersFromListQuery(ITwitterListIdentifier listIdentifier, IEnumerable<IUserIdentifier> users)
        {
            _listsQueryValidator.ThrowIfListIdentifierIsNotValid(listIdentifier);

            var listIdentifierParameter = _twitterListQueryParameterGenerator.GenerateIdentifierParameter(listIdentifier);
            var userIdsAndScreenNameParameter = _userQueryParameterGenerator.GenerateListOfUserIdentifiersParameter(users);

            var query = new StringBuilder(Resources.List_DestroyMembers);

            query.AddFormattedParameterToQuery(listIdentifierParameter);
            query.AddFormattedParameterToQuery(userIdsAndScreenNameParameter);

            return query.ToString();
        }

        public string GetCheckIfUserIsAListMemberQuery(ITwitterListIdentifier listIdentifier, IUserIdentifier user)
        {
            _listsQueryValidator.ThrowIfListIdentifierIsNotValid(listIdentifier);
            _userQueryValidator.ThrowIfUserCannotBeIdentified(user);

            var listIdentifierParameter = _twitterListQueryParameterGenerator.GenerateIdentifierParameter(listIdentifier);
            var userParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(user);

            return string.Format(Resources.List_CheckMembership, listIdentifierParameter, userParameter);
        }

        // Subscriptions
        public string GetUserSubscribedListsQuery(IUserIdentifier user, int maximumNumberOfListsToRetrieve)
        {
            _userQueryValidator.ThrowIfUserCannotBeIdentified(user);

            var userIdParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(user);
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

        public string GetCheckIfUserIsAListSubscriberQuery(ITwitterListIdentifier listIdentifier, IUserIdentifier user)
        {
            _listsQueryValidator.ThrowIfListIdentifierIsNotValid(listIdentifier);
            _userQueryValidator.ThrowIfUserCannotBeIdentified(user);

            var listIdentifierParameter = _twitterListQueryParameterGenerator.GenerateIdentifierParameter(listIdentifier);
            var userParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(user);

            var query = new StringBuilder(Resources.List_CheckSubscriber);

            query.AddFormattedParameterToQuery(listIdentifierParameter);
            query.AddFormattedParameterToQuery(userParameter);
            query.AddParameterToQuery("skip_status", "true");

            return query.ToString();
        }
    }
}