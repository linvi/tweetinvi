namespace Tweetinvi.Core.Parameters
{
    public class GetUserFavoritesParameters : CustomRequestParameters, IGetUserFavoritesParameters
    {
        public GetUserFavoritesParameters()
        {
            MaximumNumberOfTweetsToRetrieve = 200;
            IncludeEntities = true;
        }

        public int MaximumNumberOfTweetsToRetrieve { get; set; }
        public long? SinceId { get; set; }
        public long? MaxId { get; set; }
        public bool IncludeEntities { get; set; }
    }
}