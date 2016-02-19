namespace Tweetinvi.Core.Interfaces.Models
{
    /// <summary>
    /// Where On Earth ID : https://developer.yahoo.com/geo/geoplanet/guide/concepts.html
    /// </summary>
    public interface IWoeIdLocation
    {
        /// <summary>
        /// Location name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Location WOEID
        /// </summary>
        long WoeId { get; set; }
    }
}