using Tweetinvi.Core.Authentication;

namespace Tweetinvi.Core.Interfaces.Credentials
{
    public interface IWebTokenFactory
    {
        IAuthenticationContext InitAuthenticationProcess(IConsumerCredentials appCredentials, string callbackURL, string credsIdentifier);
        string GetVerifierCodeFromCallbackURL(string callbackURL);
    }
}