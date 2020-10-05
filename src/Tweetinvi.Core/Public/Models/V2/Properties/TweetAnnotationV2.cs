using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class TweetAnnotationV2
    {
        /// <summary>
        /// The start position (zero-based) of the text used to annotate the Tweet.
        /// </summary>
        [JsonProperty("end")] public int End { get; set; }

        /// <summary>
        /// The text used to determine the annotation type.
        /// </summary>
        [JsonProperty("normalized_text")] public string NormalizedText { get; set; }

        /// <summary>
        /// The confidence score for the annotation as it correlates to the Tweet text.
        /// </summary>
        [JsonProperty("probability")] public double Probability { get; set; }

        /// <summary>
        /// The start position (zero-based) of the text used to annotate the Tweet.
        /// </summary>
        [JsonProperty("start")] public int Start { get; set; }

        /// <summary>
        /// The description of the type of entity identified when the Tweet text was interpreted.
        /// </summary>
        [JsonProperty("type")] public string Type { get; set; }
    }
}