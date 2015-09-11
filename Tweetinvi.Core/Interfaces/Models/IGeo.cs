using System.Collections.Generic;

namespace Tweetinvi.Core.Interfaces.Models
{
    /// <summary>
    /// Geographic information of a location
    /// </summary>
    public interface IGeo
    {
        string Type { get; set; }
        List<ICoordinates> Coordinates { get; set; }
    }
}