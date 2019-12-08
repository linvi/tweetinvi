using Tweetinvi.Models;
using Tweetinvi.Parameters.Auth;

namespace Tweetinvi.Controllers.Auth
{
    public class RequestAuthUrlInternalParameters : RequestUrlAuthUrlParameters
    {
        public IAuthenticationToken AuthenticationToken { get; }

        public RequestAuthUrlInternalParameters(IRequestAuthUrlParameters parameters, IAuthenticationToken authenticationToken) : base(parameters)
        {
            AuthenticationToken = authenticationToken;
        }
    }
}