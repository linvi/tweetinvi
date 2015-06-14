namespace Tweetinvi.Core.Interfaces.Credentials
{
    public interface IWebTokenCreator
    {
        string GetPinCodeAuthorizationURL(ITemporaryCredentials temporaryCredentials);
        string GetAuthorizationURL(ITemporaryCredentials temporaryCredentials, string callbackURL);
        string GetVerifierCodeFromCallbackURL(string callbackURL);
    }
}