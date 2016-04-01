using Tweetinvi.Core.Authentication;

namespace Tweetinvi.Core.Interfaces.Credentials
{
    public interface IWebTokenFactory
    {
        IAuthenticationContext InitAuthenticationProcess(IConsumerCredentials appCredentials, string callbackURL, bool updateQueryIsAuthorized);
        string GetVerifierCodeFromCallbackURL(string callbackURL);
    }
}