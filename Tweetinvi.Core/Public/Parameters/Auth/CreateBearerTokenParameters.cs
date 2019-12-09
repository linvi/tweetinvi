namespace Tweetinvi.Parameters.Auth
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/basics/authentication/api-reference/token
    /// </summary>
    public interface ICreateBearerTokenParameters : ICustomRequestParameters
    {
    }

    /// <inheritdoc/>
    public class CreateBearerTokenParameters : CustomRequestParameters, ICreateBearerTokenParameters
    {
    }
}