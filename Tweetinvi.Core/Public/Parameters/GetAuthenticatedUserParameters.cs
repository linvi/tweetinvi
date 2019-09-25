using Tweetinvi.Parameters.Optionals;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://dev.twitter.com/rest/reference/get/account/verify_credentials
    /// </summary>
    public interface IGetAuthenticatedUserParameters : IGetUsersOptionalParameters
    {
        /// <summary>
        /// Include the email of the user. This is only available if the application 
        /// has been verified and approved by Twitter.
        /// </summary>
        bool? IncludeEmail { get; set; }
    }

    /// <summary>
    /// For more information visit : https://dev.twitter.com/rest/reference/get/account/verify_credentials
    /// </summary>
    public class GetAuthenticatedUserParameters : GetUsersOptionalParameters, IGetAuthenticatedUserParameters
    {
        public GetAuthenticatedUserParameters()
        {
            IncludeEmail = true;
        }

        public GetAuthenticatedUserParameters(IGetAuthenticatedUserParameters parameters) : base(parameters)
        {
            IncludeEmail = parameters?.IncludeEmail;
        }

        public bool? IncludeEmail { get; set; }
    }
}