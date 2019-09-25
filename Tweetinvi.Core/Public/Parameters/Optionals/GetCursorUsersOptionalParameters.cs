namespace Tweetinvi.Parameters.Optionals
{
    public interface IGetCursorUsersOptionalParameters : ICursorQueryParameters, IGetUsersOptionalParameters
    {
    }
    
    public class GetCursorUsersOptionalParameters : CursorQueryParameters, IGetUsersOptionalParameters
    {
        public GetCursorUsersOptionalParameters()
        {
        }

        public GetCursorUsersOptionalParameters(IGetCursorUsersOptionalParameters parameters) : base(parameters)
        {
            IncludeEntities = parameters?.IncludeEntities;
            SkipStatus = parameters?.SkipStatus;
        }        
        
        public bool? IncludeEntities { get; set; }
        public bool? SkipStatus { get; set; }
    }
}