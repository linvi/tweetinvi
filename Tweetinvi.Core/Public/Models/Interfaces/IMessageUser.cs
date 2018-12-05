namespace Tweetinvi.Models.DTO
{
    public interface IMessageUser
    {
        long Id { get; set; }
        string Name { get; set; }
        string ScreenName { get; set; }
        bool Protected { get; set; }
        bool Verified { get; set; }
        int FollowersCount { get; set; }
        int FriendsCount { get; set; }
        int StatusesCount { get; set; }
        string ProfileImageUrl { get; set; }
        string ProfileImageUrlHttps { get; set; }
        long UserCreated { get; set; }
    }
}
