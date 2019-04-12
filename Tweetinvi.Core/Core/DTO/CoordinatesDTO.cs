using System.Collections.Generic;
using Newtonsoft.Json;
using Tweetinvi.Models;

namespace Tweetinvi.Logic.DTO
{
    /// <summary>
    /// Coordinates of a geographical location
    /// </summary>
    public class CoordinatesDTO : ICoordinates
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public CoordinatesDTO() { }

        /// <summary>
        /// Create coordinates with its longitude and latitude
        /// </summary>
        public CoordinatesDTO(double longitude, double latitude)
        {
            Longitude = longitude; 
            Latitude = latitude;
        }

        [JsonProperty("coordinates")]
        private List<double> _coordinatesSetter
        {
            set
            {
                if (value != null)
                {
                    Longitude = value[0];
                    Latitude = value[1];
                }
            }
        }
    }
}