namespace Tweetinvi.Core.Interfaces.Models
{
    public interface IUserIdentifier
    {
        long Id { get; set; }
        string IdStr { get; set; }
        string ScreenName { get; set; }
    }
}