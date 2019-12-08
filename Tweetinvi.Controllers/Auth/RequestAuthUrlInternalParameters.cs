using Tweetinvi.Models;
using Tweetinvi.Parameters.Auth;

namespace Tweetinvi.Controllers.Auth
{
    public class RequestAuthUrlInternalParameters : RequestUrlAuthUrlParameters
    {
        public IAuthenticationRequestToken AuthRequestToken { get; }

        public RequestAuthUrlInternalParameters(IRequestAuthUrlParameters parameters, IAuthenticationRequestToken authRequestToken) : base(parameters)
        {
            AuthRequestToken = authRequestToken;
        }
    }
}