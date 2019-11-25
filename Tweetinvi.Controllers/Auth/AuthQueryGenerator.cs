using System.Text;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Parameters.Auth;

namespace Tweetinvi.Controllers.Auth
{
    public interface IAuthQueryGenerator
    {
        string GetCreateBearerTokenQuery();
        string GetRequestTokenQuery(IStartAuthProcessParameters parameters);
    }

    public class AuthQueryGenerator : IAuthQueryGenerator
    {
        public string GetCreateBearerTokenQuery()
        {
            return "https://api.twitter.com/oauth2/token";
        }

        public string GetRequestTokenQuery(IStartAuthProcessParameters parameters)
        {
            var query = new StringBuilder(Resources.Auth_RequestToken);

            if (parameters.AuthAccessType != null)
            {
                var paramValue = parameters.AuthAccessType == AuthAccessType.ReadWrite ? "write" : "read";
                query.AddParameterToQuery("x_auth_access_type", paramValue);
            }

            return query.ToString();
        }
    }
}