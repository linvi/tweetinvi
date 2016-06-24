namespace Tweetinvi.Streaming.Events
{
    public interface IAccessRevokedInfo
    {
        string Token { get; }
        long ApplicationId { get; }
        string ApplicationURL { get; }
        string ApplicationConsumerKey { get; }
        string ApplicationName { get; }
    }
}