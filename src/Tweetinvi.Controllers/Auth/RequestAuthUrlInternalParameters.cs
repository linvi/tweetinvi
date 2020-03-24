using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Auth
{
    public class RequestAuthUrlInternalParameters : RequestUrlAuthUrlParameters
    {
        public IAuthenticationRequest AuthRequest { get; }

        public RequestAuthUrlInternalParameters(IRequestAuthUrlParameters parameters, IAuthenticationRequest authRequest) : base(parameters)
        {
            AuthRequest = authRequest;
        }
    }
}