namespace Tweetinvi.Parameters.Auth
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/basics/authentication/api-reference/invalidate_access_token
    /// </summary>
    public interface IInvalidateAccessTokenParameters : ICustomRequestParameters
    {
    }

    /// <inheritdoc/>
    public class InvalidateAccessTokenParameters : CustomRequestParameters, IInvalidateAccessTokenParameters
    {
    }
}