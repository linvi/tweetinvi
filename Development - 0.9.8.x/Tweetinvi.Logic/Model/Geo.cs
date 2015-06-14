using System.Collections.Generic;
using Newtonsoft.Json;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Logic.Model
{
    /// <summary>
    /// Geographic information of a location
    /// </summary>
    public class Geo : IGeo
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        // ReSharper disable once UnusedMember.Local -- This is used during the deserialization
        [JsonIgnore]
        private List<ICoordinates>[] _storedCoordinates
        {
            set
            {
                if (value == null)
                {
                    Coordinates = null;
                }
                else if (value.IsEmpty())
                {
                    Coordinates = new List<ICoordinates>();
                }
                else
                {
                    Coordinates = value[0];
                }
            }
        }

        [JsonIgnore]
        public List<ICoordinates> Coordinates { get; set; }
    }
}