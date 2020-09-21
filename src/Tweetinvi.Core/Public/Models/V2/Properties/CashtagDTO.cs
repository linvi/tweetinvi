using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class CashtagDTO
    {
        [JsonProperty("start")] public int start { get; set; }

        [JsonProperty("end")] public int end { get; set; }

        [JsonProperty("cashtag")] private string cashtag
        {
            set => tag = value;
        }

        [JsonProperty("tag")] public string tag { get; set; }
    }
}