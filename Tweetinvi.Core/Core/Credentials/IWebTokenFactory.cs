namespace Tweetinvi.Core.Credentials
{
    public interface IWebTokenFactory
    {
        string GetVerifierCodeFromCallbackURL(string callbackURL);
    }
}