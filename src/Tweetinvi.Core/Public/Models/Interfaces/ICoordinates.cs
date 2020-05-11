namespace Tweetinvi.Models
{
    /// <summary>
    /// Coordinates of a geographical location.
    /// </summary>
    public interface ICoordinates
    {
        /// <summary>
        /// Longitude of the coordinate (X).
        /// </summary>
        double Longitude { get; set; }

        /// <summary>
        /// Lattitude of the coordinate (Y).
        /// </summary>
        double Latitude { get; set; }
    }
}