using System.Text;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Parameters.V2;

namespace Tweetinvi.Controllers.User
{
    public interface IUsersV2QueryGenerator
    {
        string GetUserQuery(IGetUserV2Parameters parameters);
        string GetUsersQuery(IGetUsersV2Parameters parameters);
    }

    public class UsersV2QueryGenerator : IUsersV2QueryGenerator
    {
        public string GetUserQuery(IGetUserV2Parameters parameters)
        {
            var query = new StringBuilder($"{Resources.UserV2_Get}/{parameters.UserId}");
            AddTweetFieldsParameters(parameters, query);
            return query.ToString();
        }

        public string GetUsersQuery(IGetUsersV2Parameters parameters)
        {
            var userIds = string.Join(",", parameters.UserIds);
            var query = new StringBuilder($"{Resources.UserV2_Get}");
            query.AddParameterToQuery("ids", userIds);
            AddTweetFieldsParameters(parameters, query);
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