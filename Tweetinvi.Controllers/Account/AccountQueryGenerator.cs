using System;
using System.Text;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Controllers.Shared;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Account
{
    public interface IAccountQueryGenerator
    {
        string GetAuthenticatedUserQuery(IGetAuthenticatedUserParameters parameters, TweetMode? tweetMode);
        
        // BLOCK
        string GetBlockUserQuery(IBlockUserParameters parameters);
        string GetUnblockUserQuery(IUnblockUserParameters parameters);
        string GetReportUserForSpamQuery(IReportUserForSpamParameters parameters);
        string GetBlockedUserIdsQuery(IGetBlockedUserIdsParameters parameters);
        string GetBlockedUsersQuery(IGetBlockedUsersParameters parameters);
        
        // FOLLOWERS
        string GetFollowUserQuery(IFollowUserParameters parameters);
        string GetUnFollowUserQuery(IUnFollowUserParameters parameters);
        
        // ONGOING REQUESTS
        string GetUserIdsRequestingFriendshipQuery(IGetUserIdsRequestingFriendshipParameters parameters);
        string GetUserIdsYouRequestedToFollowQuery(IGetUserIdsYouRequestedToFollowParameters parameters);


        // FRIENDSHIPS
        string GetRelationshipsWithQuery(IGetRelationshipsWithParameters parameters);








        // Profile
        string GetUpdateProfileParametersQuery(IUpdateProfileParameters parameters);
        
        // Mute
        string GetMutedUserIdsQuery();

        string GetMuteQuery(IUserIdentifier user);

        string GetUnMuteQuery(IUserIdentifier user);

        // Suggestions
        string GetSuggestedCategories(Language? language);
        string GetUserSuggestionsQuery(string slug, Language? language);
        string GetSuggestedUsersWithTheirLatestTweetQuery(string slug);
        string GetUpdateRelationshipQuery(IUpdateRelationshipParameters parameters);
        string GetUserIdsWhoseRetweetsAreMutedQuery(IGetUserIdsWhoseRetweetsAreMutedParameters parameters);
    }

    public class AccountQueryGenerator : IAccountQueryGenerator
    {
        private readonly IUserQueryParameterGenerator _userQueryParameterGenerator;
        private readonly IQueryParameterGenerator _queryParameterGenerator;

        public AccountQueryGenerator(
            IUserQueryParameterGenerator userQueryParameterGenerator,
            IQueryParameterGenerator queryParameterGenerator)
        {
            _userQueryParameterGenerator = userQueryParameterGenerator;
            _queryParameterGenerator = queryParameterGenerator;
        }

        public string GetAuthenticatedUserQuery(IGetAuthenticatedUserParameters parameters, TweetMode? tweetMode)
        {
            var query = new StringBuilder(Resources.User_GetCurrentUser);
            parameters = parameters ?? new GetAuthenticatedUserParameters();

            query.AddParameterToQuery("skip_status", parameters.SkipStatus);
            query.AddParameterToQuery("include_entities", parameters.IncludeEntities);
            query.AddParameterToQuery("include_email", parameters.IncludeEmail);
            query.AddFormattedParameterToQuery(_queryParameterGenerator.GenerateTweetModeParameter(tweetMode));
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }
        
        // BLOCK
        public string GetBlockUserQuery(IBlockUserParameters parameters)
        {
            var query = new StringBuilder(Resources.User_Block_Create);
            
            query.AddFormattedParameterToQuery(_userQueryParameterGenerator.GenerateIdOrScreenNameParameter(parameters.User));
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetUnblockUserQuery(IUnblockUserParameters parameters)
        {
            var query = new StringBuilder(Resources.User_Block_Destroy);

            query.AddFormattedParameterToQuery(_userQueryParameterGenerator.GenerateIdOrScreenNameParameter(parameters.User));
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetReportUserForSpamQuery(IReportUserForSpamParameters parameters)
        {
            var query = new StringBuilder(Resources.User_Report_Spam);

            query.AddFormattedParameterToQuery(_userQueryParameterGenerator.GenerateIdOrScreenNameParameter(parameters.User));
            query.AddParameterToQuery("perform_block", parameters.PerformBlock);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetBlockedUserIdsQuery(IGetBlockedUserIdsParameters parameters)
        {
            var query = new StringBuilder(Resources.User_Block_List_Ids);

            query.AddParameterToQuery("cursor", parameters.Cursor);
            query.AddParameterToQuery("count", parameters.PageSize);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetBlockedUsersQuery(IGetBlockedUsersParameters parameters)
        {
            var query = new StringBuilder(Resources.User_Block_List);

            query.AddParameterToQuery("cursor", parameters.Cursor);
            query.AddParameterToQuery("count", parameters.PageSize);
            query.AddParameterToQuery("include_entities", parameters.IncludeEntities);
            query.AddParameterToQuery("skip_status",  parameters.SkipStatus);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }
        
        // FOLLOWERS
        public string GetFollowUserQuery(IFollowUserParameters parameters)
        {
            var query = new StringBuilder(Resources.Friendship_Create);

            query.AddFormattedParameterToQuery(_userQueryParameterGenerator.GenerateIdOrScreenNameParameter(parameters.User));
            query.AddParameterToQuery("follow", parameters.EnableNotifications);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }
        
        public string GetUpdateRelationshipQuery(IUpdateRelationshipParameters parameters)
        {
            var query = new StringBuilder(Resources.Friendship_Update);

            query.AddFormattedParameterToQuery(_userQueryParameterGenerator.GenerateIdOrScreenNameParameter(parameters.User));
            query.AddParameterToQuery("device", parameters.EnableDeviceNotifications);
            query.AddParameterToQuery("retweets", parameters.EnableRetweets);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetUnFollowUserQuery(IUnFollowUserParameters parameters)
        {
            var query = new StringBuilder(Resources.Friendship_Destroy);

            query.AddFormattedParameterToQuery(_userQueryParameterGenerator.GenerateIdOrScreenNameParameter(parameters.User));
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }
        
        public string GetUserIdsRequestingFriendshipQuery(IGetUserIdsRequestingFriendshipParameters parameters)
        {
            var query = new StringBuilder(Resources.Friendship_GetIncomingIds);
            
            query.AddParameterToQuery("cursor", parameters.Cursor);
            query.AddParameterToQuery("count", parameters.PageSize);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetUserIdsYouRequestedToFollowQuery(IGetUserIdsYouRequestedToFollowParameters parameters)
        {
            var query = new StringBuilder(Resources.Friendship_GetOutgoingIds);
            
            query.AddParameterToQuery("cursor", parameters.Cursor);
            query.AddParameterToQuery("count", parameters.PageSize);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetRelationshipsWithQuery(IGetRelationshipsWithParameters parameters)
        {
            var query = new StringBuilder(Resources.Friendship_GetRelationships);

            query.AddFormattedParameterToQuery(_userQueryParameterGenerator.GenerateListOfUserIdentifiersParameter(parameters.Users));
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetUserIdsWhoseRetweetsAreMutedQuery(IGetUserIdsWhoseRetweetsAreMutedParameters parameters)
        {
            var query = new StringBuilder(Resources.Friendship_FriendIdsWithNoRetweets);

            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);
            
            return query.ToString();
        }






        

        
         // Profile
        public string GetUpdateProfileParametersQuery(IUpdateProfileParameters parameters)
        {
            var query = new StringBuilder(Resources.Account_UpdateProfile);

            query.AddParameterToQuery("name", parameters.Name);
            query.AddParameterToQuery("url", parameters.WebsiteUrl);
            query.AddParameterToQuery("location", parameters.Location);
            query.AddParameterToQuery("description", parameters.Description);
            query.AddParameterToQuery("profile_link_color", parameters.ProfileLinkColor);
            query.AddParameterToQuery("include_entities", parameters.IncludeEntities);
            query.AddParameterToQuery("skip_status", parameters.SkipStatus);

            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        // Mute
        public string GetMutedUserIdsQuery()
        {
            return Resources.Account_Mute_GetIds;
        }

        public string GetMuteQuery(IUserIdentifier user)
        {
            string userIdParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(user);
            return GenerateCreateMuteQuery(userIdParameter);
        }

        private string GenerateCreateMuteQuery(string userParameter)
        {
            return string.Format(Resources.Account_Mute_Create, userParameter);
        }

        public string GetUnMuteQuery(IUserIdentifier user)
        {
            string userIdParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(user);
            return GenerateUnMuteQuery(userIdParameter);
        }

        private string GenerateUnMuteQuery(string userParameter)
        {
            return string.Format(Resources.Account_Mute_Destroy, userParameter);
        }

        // Suggestions
        public string GetSuggestedCategories(Language? language)
        {
            var languageParameter = _queryParameterGenerator.GenerateLanguageParameter(language);

            return string.Format(Resources.Account_CategoriesSuggestions, languageParameter);
        }

        public string GetUserSuggestionsQuery(string slug, Language? language)
        {
            if (slug == null)
            {
                throw new ArgumentNullException("Slug cannot be null.");
            }

            if (slug == "")
            {
                throw new ArgumentException("Slug cannot be empty.");
            }

            var languageParameter = _queryParameterGenerator.GenerateLanguageParameter(language);

            return string.Format(Resources.Account_UserSuggestions, slug, languageParameter);
        }

        public string GetSuggestedUsersWithTheirLatestTweetQuery(string slug)
        {
            if (slug == null)
            {
                throw new ArgumentNullException("Slug cannot be null.");
            }

            if (slug == "")
            {
                throw new ArgumentException("Slug cannot be empty.");
            }

            return string.Format(Resources.Account_MembersSuggestions, slug);
        }
    }
}