using System;
using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class PollDTO
    {
        [JsonProperty("duration_minutes")] public int duration_minutes { get; set; }

        [JsonProperty("end_datetime")] public DateTimeOffset end_datetime { get; set; }

        [JsonProperty("id")] public string id { get; set; }

        [JsonProperty("options")] public PollOptionDTO[] options { get; set; }

        [JsonProperty("voting_status")] public string voting_status { get; set; }
    }

    public class PollOptionDTO
    {
        [JsonProperty("label")] public string label { get; set; }

        [JsonProperty("position")] public int position { get; set; }

        [JsonProperty("votes")] public int votes { get; set; }
    }
}