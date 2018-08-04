namespace Tweetinvi.Core.Public.Streaming.Events
{
    public interface IActivityStreamAppIdentifierDTO
    {
        long AppId { get; set; }
    }

    public interface IActivityStreamUserIdentifierDTO
    {
        long UserId { get; set; }
    }

    public interface IUserRevokedAppPermissionsDTO
    {
        IActivityStreamAppIdentifierDTO Target { get; set; }
        IActivityStreamUserIdentifierDTO Source { get; set; }
    }
}
