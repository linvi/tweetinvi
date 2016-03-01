using Tweetinvi.Core.Enum;

namespace Tweetinvi.Core.Interfaces.Models
{
    public interface ITrendLocation
    {
        /// <summary>
        /// Trend location : Where On Earth ID : https://developer.yahoo.com/geo/geoplanet/guide/concepts.html
        /// </summary>
        long WoeId { get; set; }

        /// <summary>
        /// Trend name.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Country where this trend is active.
        /// </summary>
        string Country { get; set; }

        /// <summary>
        /// Country code where this trend is active.
        /// </summary>
        string CountryCode { get; set; }

        /// <summary>
        /// Search url containing tweets with the trend.
        /// </summary>
        string Url { get; set; }

        /// <summary>
        /// WOEID of the parent location.
        /// E.G NewYork parentId would be United States
        /// </summary>
        long ParentId { get; set; }

        /// <summary>
        /// Type/Size of the referenced place.
        /// </summary>
        PlaceType PlaceType { get; set; }
    }
}