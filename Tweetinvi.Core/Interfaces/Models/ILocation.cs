namespace Tweetinvi.Core.Interfaces.Models
{
    /// <summary>
    /// A rectangle box area defined by two coordinates.
    /// </summary>
    public interface ILocation
    {
        /// <summary>
        /// First coordinate of the box. For simplicity use (top, left).
        /// </summary>
        ICoordinates Coordinate1 { get; set; }

        /// <summary>
        /// Second coordinate of the box. For simplicity use (bottom, right).
        /// </summary>
        ICoordinates Coordinate2 { get; set; }
    }
}