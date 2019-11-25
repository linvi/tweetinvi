using Tweetinvi.Models;
using Tweetinvi.Parameters.Auth;

namespace Tweetinvi.Controllers.Auth
{
    public class StartAuthProcessInternalParameters : StartUrlAuthProcessParameters
    {
        public IAuthenticationToken AuthenticationToken { get; }

        public StartAuthProcessInternalParameters(IStartAuthProcessParameters parameters, IAuthenticationToken authenticationToken) : base(parameters)
        {
            AuthenticationToken = authenticationToken;
        }
    }
}