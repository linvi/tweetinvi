using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class CashtagV2
    {
        /// <summary>
        /// Index of the first letter of the cashtag
        /// </summary>
        [JsonProperty("start")] public int Start { get; set; }

        /// <summary>
        /// Index of the last letter of the cashtag
        /// </summary>
        [JsonProperty("end")] public int End { get; set; }

        /// <summary>
        /// The text of the Cashtag.
        /// </summary>
        [JsonProperty("tag")] public string Tag { get; set; }

        [JsonProperty("cashtag")] private string Cashtag
        {
            set => Tag = value;
        }
    }
}