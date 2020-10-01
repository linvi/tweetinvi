using System;
using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    public class PollDTO
    {
        [JsonProperty("duration_minutes")] public int DurationMinutes { get; set; }
        [JsonProperty("end_datetime")] public DateTimeOffset EndDate { get; set; }
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("options")] public PollOptionDTO[] PollOptions { get; set; }
        [JsonProperty("voting_status")] public string VotingStatus { get; set; }
    }

    public class PollOptionDTO
    {
        [JsonProperty("label")] public string Label { get; set; }
        [JsonProperty("position")] public int Position { get; set; }
        [JsonProperty("votes")] public int Votes { get; set; }
    }
}