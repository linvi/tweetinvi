using System.Text;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Controllers.Shared;
using Tweetinvi.Core;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.QueryValidators;
using Tweetinvi.Models;
using Tweetinvi.Parameters.ListsClient;

namespace Tweetinvi.Controllers.TwitterLists
{
    public interface ITwitterListQueryGenerator
    {
        // list
        string GetCreateListQuery(ICreateListParameters parameters);
        string GetListQuery(IGetListParameters parameters);
        string GetListsSubscribedByUserQuery(IGetListsSubscribedByUserParameters parameters);
        string GetUpdateListQuery(IUpdateListParameters parameters);
        string GetDestroyListQuery(IDestroyListParameters parameters);
        string GetListsOwnedByUserQuery(IGetListsOwnedByUserParameters parameters);

        // members
        string GetAddMemberToListQuery(IAddMemberToListParameters parameters);
        string GetAddMembersQuery(IAddMembersToListParameters parameters);
        string GetCheckIfUserIsMemberOfListQuery(ICheckIfUserIsMemberOfListParameters parameters);
        string GetListsAUserIsMemberOfQuery(IGetListsAUserIsMemberOfParameters parameters);
        string GetMembersOfListQuery(IGetMembersOfListParameters parameters);
        string GetRemoveMemberFromListParameter(IRemoveMemberFromListParameters parameters);
        string GetRemoveMembersFromListParameters(IRemoveMembersFromListParameters parameters);





        // old
        string GetUserSubscribedListsQuery(IUserIdentifier user, bool getOwnedListsFirst);


        string GetTweetsFromListQuery(IGetTweetsFromListQueryParameters queryParameters);

        string GetUserSubscribedListsQuery(IUserIdentifier user, int maximumNumberOfListsToRetrieve);

        string GetListSubscribersQuery(ITwitterListIdentifier listIdentifier, int maximumNumberOfSubscribersToRetrieve);
        string GetSubscribeUserToListQuery(ITwitterListIdentifier listIdentifier);
        string GetUnSubscribeUserFromListQuery(ITwitterListIdentifier listIdentifier);
        string GetCheckIfUserIsAListSubscriberQuery(ITwitterListIdentifier listIdentifier, IUserIdentifier user);
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
        public string GetCreateListQuery(ICreateListParameters parameters)
        {
            var query = new StringBuilder(Resources.List_Create);

            AppendListMetadataToQuery(parameters, query);

            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        private static void AppendListMetadataToQuery(IListMetadataParameters parameters, StringBuilder query)
        {
            query.AddParameterToQuery("name", parameters.Name);
            query.AddParameterToQuery("mode", parameters.PrivacyMode?.ToString()?.ToLowerInvariant());
            query.AddParameterToQuery("description", parameters.Description);
        }

        public string GetListQuery(IGetListParameters parameters)
        {
            var query = new StringBuilder(Resources.List_Get);

            _twitterListQueryParameterGenerator.AppendListIdentifierParameter(query, parameters.List);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetListsSubscribedByUserQuery(IGetListsSubscribedByUserParameters parameters)
        {
            var query = new StringBuilder(Resources.List_GetUserLists);

            query.AddFormattedParameterToQuery(_userQueryParameterGenerator.GenerateIdOrScreenNameParameter(parameters.User));
            query.AddParameterToQuery("reverse", parameters.Reverse);

            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetUpdateListQuery(IUpdateListParameters parameters)
        {
            var query = new StringBuilder(Resources.List_Update);

            _twitterListQueryParameterGenerator.AppendListIdentifierParameter(query, parameters.List);

            AppendListMetadataToQuery(parameters, query);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetDestroyListQuery(IDestroyListParameters parameters)
        {
            var query = new StringBuilder(Resources.List_Destroy);

            _twitterListQueryParameterGenerator.AppendListIdentifierParameter(query, parameters.List);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetListsOwnedByUserQuery(IGetListsOwnedByUserParameters parameters)
        {
            var query = new StringBuilder(Resources.List_OwnedByUser);

            query.AddFormattedParameterToQuery(_userQueryParameterGenerator.GenerateIdOrScreenNameParameter(parameters.User));

            _queryParameterGenerator.AppendCursorParameters(query, parameters);

            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetAddMemberToListQuery(IAddMemberToListParameters parameters)
        {
            var query = new StringBuilder(Resources.List_Members_Create);

            _twitterListQueryParameterGenerator.AppendListIdentifierParameter(query, parameters.List);
            _userQueryParameterGenerator.AppendUser(query, parameters.User);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetAddMembersQuery(IAddMembersToListParameters parameters)
        {
            var query = new StringBuilder(Resources.List_CreateMembers);

            _twitterListQueryParameterGenerator.AppendListIdentifierParameter(query, parameters.List);
            _userQueryParameterGenerator.AppendUsers(query, parameters.Users);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetCheckIfUserIsMemberOfListQuery(ICheckIfUserIsMemberOfListParameters parameters)
        {
            var query = new StringBuilder(Resources.List_CheckMembership);

            _twitterListQueryParameterGenerator.AppendListIdentifierParameter(query, parameters.List);
            _userQueryParameterGenerator.AppendUser(query, parameters.User);

            query.AddParameterToQuery("include_entities", parameters.IncludeEntities);
            query.AddParameterToQuery("skip_status", parameters.SkipStatus);

            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetListsAUserIsMemberOfQuery(IGetListsAUserIsMemberOfParameters parameters)
        {
            var query = new StringBuilder(Resources.List_GetUserMemberships);

            _userQueryParameterGenerator.AppendUser(query, parameters.User);
            _queryParameterGenerator.AppendCursorParameters(query, parameters);
            query.AddParameterToQuery("filter_to_owned_lists", parameters.OnlyRetrieveAccountLists);

            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetMembersOfListQuery(IGetMembersOfListParameters parameters)
        {
            var query = new StringBuilder(Resources.List_Members_List);

            _twitterListQueryParameterGenerator.AppendListIdentifierParameter(query, parameters.List);
            _queryParameterGenerator.AppendCursorParameters(query, parameters);

            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetRemoveMemberFromListParameter(IRemoveMemberFromListParameters parameters)
        {
            var query = new StringBuilder(Resources.List_DestroyMember);

            _twitterListQueryParameterGenerator.AppendListIdentifierParameter(query, parameters.List);
            _userQueryParameterGenerator.AppendUser(query, parameters.User);

            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetRemoveMembersFromListParameters(IRemoveMembersFromListParameters parameters)
        {
            var query = new StringBuilder(Resources.List_DestroyMembers);

            _twitterListQueryParameterGenerator.AppendListIdentifierParameter(query, parameters.List);
            _userQueryParameterGenerator.AppendUsers(query, parameters.Users);

            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }
















        public string GetUserSubscribedListsQuery(IUserIdentifier user, bool getOwnedListsFirst)
        {
            _userQueryValidator.ThrowIfUserCannotBeIdentified(user);

            var identifierParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(user);
            return string.Format(Resources.List_GetUserLists, identifierParameter, getOwnedListsFirst);
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