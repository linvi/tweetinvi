namespace Tweetinvi.Models.DTO
{
    /// <summary>
    /// DTO for encapsulating an MessageEvent and an App together for storage.
    /// Not used for transfer to or from Twitter.
    /// </summary>
    public interface IMessageEventWithAppDTO
    {
        IMessageEventDTO MessageEvent { get; set; }
        IApp App { get; set; }
    }
}
