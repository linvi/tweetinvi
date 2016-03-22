using Tweetinvi.Core.Credentials;

namespace Tweetinvi.Core.Interfaces.Credentials
{
    public interface IWebTokenCreator
    {
        IAuthenticationContext InitAuthenticationProcess(IConsumerCredentials appCredentials, string callbackURL, bool updateQueryIsAuthorized);
        string GetVerifierCodeFromCallbackURL(string callbackURL);
    }
}