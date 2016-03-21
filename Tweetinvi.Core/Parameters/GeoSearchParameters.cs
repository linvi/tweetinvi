using System.Collections.Generic;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Parameters
{
    public enum Granularity
    {
        Undefined,
        // ReSharper disable once InconsistentNaming
        POI,
        Neighborhood,
        City,
        Admin,
        Country
    }

    public interface IGeoSearchParameters : ICustomRequestParameters
    {
        ICoordinates Coordinates { get; set; }
        string Query { get; set; }
        string IP { get; set; }
        Granularity Granularity { get; set; }
        int? Accuracy { get; set; }
        int? MaximumNumberOfResults { get; set; }
        string ContainedWithin { get; set; }

        Dictionary<string, string> Attributes { get; }
        string Callback { get; set; }
    }

    public class GeoSearchParameters : CustomRequestParameters, IGeoSearchParameters
    {
        public GeoSearchParameters()
        {
            Attributes = new Dictionary<string, string>();
        }

        public ICoordinates Coordinates { get; set; }
        public string Query { get; set; }
        public string IP { get; set; }
        public Granularity Granularity { get; set; }
        public int? Accuracy { get; set; }
        public int? MaximumNumberOfResults { get; set; }
        public string ContainedWithin { get; set; }
        public Dictionary<string, string> Attributes { get; }
        public string Callback { get; set; }
    }
}
