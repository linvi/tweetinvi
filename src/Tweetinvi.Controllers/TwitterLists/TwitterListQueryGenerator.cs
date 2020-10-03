using System.Text;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Controllers.Shared;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Parameters;

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
        string GetUserListMembershipsQuery(IGetUserListMembershipsParameters parameters);
        string GetMembersOfListQuery(IGetMembersOfListParameters parameters);
        string GetRemoveMemberFromListQuery(IRemoveMemberFromListParameters parameters);
        string GetRemoveMembersFromListQuery(IRemoveMembersFromListParameters parameters);

        // subscribers
        string GetSubscribeToListQuery(ISubscribeToListParameters parameters);
        string GetListSubscribersQuery(IGetListSubscribersParameters parameters);
        string GetCheckIfUserIsSubscriberOfListQuery(ICheckIfUserIsSubscriberOfListParameters parameters);
        string GetUserListSubscriptionsQuery(IGetUserListSubscriptionsParameters parameters);
        string GetUnsubscribeFromListQuery(IUnsubscribeFromListParameters parameters);

        // Tweets
        string GetTweetsFromListQuery(IGetTweetsFromListParameters queryParameters, ComputedTweetMode tweetMode);
    }

    public class TwitterListQueryGenerator : ITwitterListQueryGenerator
    {
        private readonly IUserQueryParameterGenerator _userQueryParameterGenerator;
        private readonly IQueryParameterGenerator _queryParameterGenerator;
        private readonly ITwitterListQueryParameterGenerator _twitterListQueryParameterGenerator;

        public TwitterListQueryGenerator(
            IUserQueryParameterGenerator userQueryParameterGenerator,
            IQueryParameterGenerator queryParameterGenerator,
            ITwitterListQueryParameterGenerator twitterListQueryParameterGenerator)
        {
            _userQueryParameterGenerator = userQueryParameterGenerator;
            _queryParameterGenerator = queryParameterGenerator;
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

            _twitterListQueryParameterGenerator.AppendListIdentifierParameter(query, parameters);
            _userQueryParameterGenerator.AppendUser(query, parameters.User);

            query.AddParameterToQuery("include_entities", parameters.IncludeEntities);
            query.AddParameterToQuery("skip_status", parameters.SkipStatus);

            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetUserListMembershipsQuery(IGetUserListMembershipsParameters parameters)
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

            query.AddParameterToQuery("include_entities", parameters.IncludeEntities);
            query.AddParameterToQuery("skip_status", parameters.SkipStatus);

            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetRemoveMemberFromListQuery(IRemoveMemberFromListParameters parameters)
        {
            var query = new StringBuilder(Resources.List_DestroyMember);

            _twitterListQueryParameterGenerator.AppendListIdentifierParameter(query, parameters.List);
            _userQueryParameterGenerator.AppendUser(query, parameters.User);

            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetRemoveMembersFromListQuery(IRemoveMembersFromListParameters parameters)
        {
            var query = new StringBuilder(Resources.List_DestroyMembers);

            _twitterListQueryParameterGenerator.AppendListIdentifierParameter(query, parameters.List);
            _userQueryParameterGenerator.AppendUsers(query, parameters.Users);

            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        // SUBSCRIBERS
        public string GetSubscribeToListQuery(ISubscribeToListParameters parameters)
        {
            var query = new StringBuilder(Resources.List_Subscribe);

            _twitterListQueryParameterGenerator.AppendListIdentifierParameter(query, parameters);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetListSubscribersQuery(IGetListSubscribersParameters parameters)
        {
            var query = new StringBuilder(Resources.List_GetSubscribers);

            _twitterListQueryParameterGenerator.AppendListIdentifierParameter(query, parameters);
            _queryParameterGenerator.AppendCursorParameters(query, parameters);

            query.AddParameterToQuery("include_entities", parameters.IncludeEntities);
            query.AddParameterToQuery("skip_status", parameters.SkipStatus);

            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetCheckIfUserIsSubscriberOfListQuery(ICheckIfUserIsSubscriberOfListParameters parameters)
        {
            var query = new StringBuilder(Resources.List_CheckSubscriber);

            _twitterListQueryParameterGenerator.AppendListIdentifierParameter(query, parameters);
            _userQueryParameterGenerator.AppendUser(query, parameters.User);

            query.AddParameterToQuery("include_entities", parameters.IncludeEntities);
            query.AddParameterToQuery("skip_status", parameters.SkipStatus);

            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetUserListSubscriptionsQuery(IGetUserListSubscriptionsParameters parameters)
        {
            var query = new StringBuilder(Resources.List_UserSubscriptions);

            _userQueryParameterGenerator.AppendUser(query, parameters.User);
            _queryParameterGenerator.AppendCursorParameters(query, parameters);

            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetUnsubscribeFromListQuery(IUnsubscribeFromListParameters parameters)
        {
            var query = new StringBuilder(Resources.List_Unsubscribe);

            _twitterListQueryParameterGenerator.AppendListIdentifierParameter(query, parameters);

            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        // TWEETS
        public string GetTweetsFromListQuery(IGetTweetsFromListParameters parameters, ComputedTweetMode tweetMode)
        {
            var query = new StringBuilder(Resources.List_GetTweetsFromList);

            _twitterListQueryParameterGenerator.AppendListIdentifierParameter(query, parameters.List);
            _queryParameterGenerator.AddTimelineParameters(query, parameters, tweetMode);

            query.AddParameterToQuery("include_rts", parameters.IncludeRetweets);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }
    }
}