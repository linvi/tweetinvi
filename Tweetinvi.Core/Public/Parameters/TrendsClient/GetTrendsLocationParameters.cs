namespace Tweetinvi.Parameters.TrendsClient
{
    /// <summary>
    /// https://developer.twitter.com/en/docs/trends/locations-with-trending-topics/api-reference/get-trends-available
    /// </summary>
    public interface IGetTrendsLocationParameters : ICustomRequestParameters
    {
    }

    /// <inheritdoc />
    public class GetTrendsLocationParameters : CustomRequestParameters, IGetTrendsLocationParameters
    {
    }
}