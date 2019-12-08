using System.Text;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Parameters.Auth;

namespace Tweetinvi.Controllers.Auth
{
    public interface IAuthQueryGenerator
    {
        string GetCreateBearerTokenQuery();
        string GetRequestAuthUrlQuery(IRequestAuthUrlParameters parameters);
        string GetRequestCredentialsQuery(IRequestCredentialsParameters parameters);
    }

    public class AuthQueryGenerator : IAuthQueryGenerator
    {
        public string GetCreateBearerTokenQuery()
        {
            return Resources.Auth_CreateBearerToken;
        }

        public string GetRequestAuthUrlQuery(IRequestAuthUrlParameters parameters)
        {
            var query = new StringBuilder(Resources.Auth_RequestToken);

            if (parameters.AuthAccessType != null)
            {
                var paramValue = parameters.AuthAccessType == AuthAccessType.ReadWrite ? "write" : "read";
                query.AddParameterToQuery("x_auth_access_type", paramValue);
            }

            return query.ToString();
        }

        public string GetRequestCredentialsQuery(IRequestCredentialsParameters parameters)
        {
            return Resources.Auth_RequestAccessToken;
        }
    }
}