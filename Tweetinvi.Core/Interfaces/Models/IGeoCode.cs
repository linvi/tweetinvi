using Tweetinvi.Core.Enum;

namespace Tweetinvi.Core.Interfaces.Models
{
    /// <summary>
    /// GeoCode represent an area around a specific center coordinate.
    /// </summary>
    public interface IGeoCode
    {
        /// <summary>
        /// Center of the area.
        /// </summary>
        ICoordinates Coordinates { get; set; }

        /// <summary>
        /// Distance between the center and the end of the area.
        /// </summary>
        double Radius { get; set; }

        /// <summary>
        /// Type of measure used for the Radius.
        /// </summary>
        DistanceMeasure DistanceMeasure { get; set; }
    }
}