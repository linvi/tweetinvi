using System.Text;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Parameters.V2;

namespace Tweetinvi.Controllers.User
{
    public interface IUsersV2QueryGenerator
    {
        string GetUserQuery(IGetUserByIdV2Parameters parameters);
        string GetUsersQuery(IGetUsersByIdV2Parameters parameters);

        string GetUserQuery(IGetUserByUsernameV2Parameters parameters);
        string GetUsersQuery(IGetUsersByUsernameV2Parameters parameters);
    }

    public class UsersV2QueryGenerator : IUsersV2QueryGenerator
    {
        public string GetUserQuery(IGetUserByIdV2Parameters parameters)
        {
            var query = new StringBuilder($"{Resources.UserV2_Get}/{parameters.UserId}");
            AddTweetFieldsParameters(parameters, query);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);
            return query.ToString();
        }

        public string GetUsersQuery(IGetUsersByIdV2Parameters parameters)
        {
            var userIds = string.Join(",", parameters.UserIds);
            var query = new StringBuilder($"{Resources.UserV2_Get}");
            query.AddParameterToQuery("ids", userIds);
            AddTweetFieldsParameters(parameters, query);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);
            return query.ToString();
        }

        public string GetUserQuery(IGetUserByUsernameV2Parameters parameters)
        {
            var query = new StringBuilder($"{Resources.UserV2_GetBy}/{parameters.By}/{parameters.Username}");
            AddTweetFieldsParameters(parameters, query);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);
            return query.ToString();
        }

        public string GetUsersQuery(IGetUsersByUsernameV2Parameters parameters)
        {
            var userIds = string.Join(",", parameters.Usernames);
            var query = new StringBuilder($"{Resources.UserV2_GetBy}");
            query.AddParameterToQuery($"{parameters.By}s", userIds);
            AddTweetFieldsParameters(parameters, query);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);
            return query.ToString();
        }

        private static void AddTweetFieldsParameters(IBaseUsersV2Parameters parameters, StringBuilder query)
        {
            query.AddParameterToQuery("expansions", parameters.Expansions);
            query.AddParameterToQuery("tweet.fields", parameters.TweetFields);
            query.AddParameterToQuery("user.fields", parameters.UserFields);
        }
    }
}