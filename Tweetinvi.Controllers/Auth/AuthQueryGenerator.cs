namespace Tweetinvi.Controllers.Auth
{
    public interface IAuthQueryGenerator
    {
        string GetCreateBearerTokenQuery();
    }

    public class AuthQueryGenerator : IAuthQueryGenerator
    {
        public string GetCreateBearerTokenQuery()
        {
            return "https://api.twitter.com/oauth2/token";
        }
    }
}