using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Parameters
{
    /// <summary>
    /// https://dev.twitter.com/rest/reference/get/geo/reverse_geocode
    /// </summary>
    public interface IGeoSearchReverseParameters : ICustomRequestParameters
    {
        /// <summary>
        /// Coordinates of the geo location.
        /// </summary>
        ICoordinates Coordinates { get; set; }

        /// <summary>
        /// This is the minimal granularity of place types to return.
        /// </summary>
        Granularity Granularity { get; set; }

        /// <summary>
        /// A hint on the “region” in which to search. If a number, then this is a radius in meters, 
        /// but it can also take a string that is suffixed with ft to specify feet.
        /// </summary>
        int? Accuracy { get; set; }

        /// <summary>
        /// Maximum number of places to return.
        /// </summary>
        int? MaximumNumberOfResults { get; set; }

        /// <summary>
        /// If supplied, the response will use the JSONP format with a callback of the given name.
        /// </summary>
        string Callback { get; set; }
    }

    /// <summary>
    /// https://dev.twitter.com/rest/reference/get/geo/reverse_geocode
    /// </summary>
    public class GeoSearchReverseParameters : CustomRequestParameters, IGeoSearchReverseParameters
    {
        public ICoordinates Coordinates { get; set; }
        public Granularity Granularity { get; set; }
        public int? Accuracy { get; set; }
        public int? MaximumNumberOfResults { get; set; }
        public string Callback { get; set; }
    }
}