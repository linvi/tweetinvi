using System.Collections.Generic;
using Tweetinvi.Core.Enum;

namespace Tweetinvi.Core.Interfaces.Models
{
    public interface IPlace
    {
        string IdStr { get; }
        string Name { get; set; }
        string FullName { get; set; }

        string Url { get; set; }
        PlaceType PlaceType { get; set; }
        string Country { get; set; }
        string CountryCode { get; set; }

        Dictionary<string, string> Attributes { get; set; }

        List<IPlace> ContainedWithin { get; set; }

        IGeo BoundingBox { get; set; }
        IGeo Geometry { get; set; }
    }
}