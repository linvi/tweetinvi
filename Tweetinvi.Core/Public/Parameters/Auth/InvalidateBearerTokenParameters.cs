namespace Tweetinvi.Parameters.Auth
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/basics/authentication/api-reference/invalidate_bearer_token
    /// </summary>
    public interface IInvalidateBearerTokenParameters : ICustomRequestParameters
    {
    }

    /// <inheritdoc/>
    public class InvalidateBearerTokenParameters : CustomRequestParameters, IInvalidateBearerTokenParameters
    {
    }
}