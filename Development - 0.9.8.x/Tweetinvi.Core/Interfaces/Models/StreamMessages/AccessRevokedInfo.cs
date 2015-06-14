namespace Tweetinvi.Core.Interfaces.DTO.StreamMessages
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