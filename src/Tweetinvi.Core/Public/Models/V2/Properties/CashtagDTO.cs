using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class CashtagDTO
    {
        [JsonProperty("start")] public int Start { get; set; }
        [JsonProperty("end")] public int End { get; set; }
        [JsonProperty("tag")] public string Tag { get; set; }

        [JsonProperty("cashtag")] private string Cashtag
        {
            set => Tag = value;
        }
    }
}