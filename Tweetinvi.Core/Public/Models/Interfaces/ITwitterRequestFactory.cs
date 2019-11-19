namespace Tweetinvi.Models
{
    public interface ITwitterRequestFactory
    {
        ITwitterRequest Create(ITwitterCredentials credentials);
    }
}
