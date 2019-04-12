namespace Tweetinvi.Models.Interfaces
{
    public interface ITwitterRequestFactory
    {
        ITwitterRequest Create(ITwitterCredentials credentials);
    }
}
