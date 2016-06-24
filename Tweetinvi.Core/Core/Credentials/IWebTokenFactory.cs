using Tweetinvi.Models;

namespace Tweetinvi.Core.Credentials
{
    public interface IWebTokenFactory
    {
        IAuthenticationContext InitAuthenticationProcess(IConsumerCredentials appCredentials, string callbackURL, string credsIdentifier);
        string GetVerifierCodeFromCallbackURL(string callbackURL);
    }
}