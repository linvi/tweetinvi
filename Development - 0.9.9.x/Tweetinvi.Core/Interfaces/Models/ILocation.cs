namespace Tweetinvi.Core.Interfaces.Models
{
    public interface ILocation
    {
        ICoordinates Coordinate1 { get; set; }
        ICoordinates Coordinate2 { get; set; }
    }
}