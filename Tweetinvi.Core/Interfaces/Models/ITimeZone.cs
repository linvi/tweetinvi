namespace Tweetinvi.Core.Interfaces.Models
{
    public interface ITimeZone
    {
        string Name { get; set; }
        string TzinfoName { get; set; }
        int UtcOffset { get; set; }
    }
}