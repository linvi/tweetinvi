namespace Tweetinvi.Parameters.Optionals
{
    public interface IGetUsersOptionalParameters : ICustomRequestParameters
    {
        /// <summary>
        /// Specifies if you want the user object to contain the user's latest tweets.
        /// </summary>
        bool? SkipStatus { get; set; }
        
        /// <summary>
        /// Include user entities.
        /// </summary>
        bool? IncludeEntities { get; set; }
    }
    
    public class GetUsersOptionalParameters : CustomRequestParameters, IGetUsersOptionalParameters
    {
        public GetUsersOptionalParameters()
        {
        }

        public GetUsersOptionalParameters(IGetUsersOptionalParameters parameters) : base(parameters)
        {
            IncludeEntities = parameters?.IncludeEntities;
            SkipStatus = parameters?.SkipStatus;
        }        
        
        public bool? IncludeEntities { get; set; }
        public bool? SkipStatus { get; set; }
    }
}