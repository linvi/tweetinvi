using Tweetinvi.Core.Enum;

namespace Tweetinvi.Core.Interfaces.Models
{
    public interface ITrendLocation
    {
        long WoeId { get; set; }
        string Name { get; set; }
        string Country { get; set; }
        string CountryCode { get; set; }
        string Url { get; set; }
        long ParentId { get; set; }
        PlaceType PlaceType { get; set; }
    }
}