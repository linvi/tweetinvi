using System.Threading.Tasks;
using Tweetinvi.Models;

namespace Tweetinvi.Core.Credentials
{
    public interface IWebTokenFactory
    {
        Task<IAuthenticationContext> InitAuthenticationProcess(IConsumerCredentials appCredentials, string callbackURL, string credsIdentifier);
        string GetVerifierCodeFromCallbackURL(string callbackURL);
    }
}