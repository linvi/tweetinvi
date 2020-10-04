using System;
using Newtonsoft.Json;

namespace Tweetinvi.Models.V2
{
    /// <summary>
    /// Poll included in a Tweet
    /// <para>Read more here : https://developer.twitter.com/en/docs/twitter-api/data-dictionary/object-model/poll </para>
    /// </summary>
    public class PollV2
    {
        /// <summary>
        /// Specifies the total duration of this poll.
        /// </summary>
        [JsonProperty("duration_minutes")] public int DurationMinutes { get; set; }

        /// <summary>
        /// Specifies the end date and time for this poll.
        /// </summary>
        [JsonProperty("end_datetime")] public DateTimeOffset EndDate { get; set; }

        /// <summary>
        /// Unique identifier of the expanded poll.
        /// </summary>
        [JsonProperty("id")] public string Id { get; set; }

        /// <summary>
        /// Contains objects describing each choice in the referenced poll.
        /// </summary>
        [JsonProperty("options")] public PollOptionV2[] PollOptions { get; set; }

        /// <summary>
        /// Indicates if this poll is still active and can receive votes, or if the voting is now closed.
        /// </summary>
        [JsonProperty("voting_status")] public string VotingStatus { get; set; }
    }

    public class PollOptionV2
    {
        /// <summary>
        /// Label of a poll option
        /// </summary>
        [JsonProperty("label")] public string Label { get; set; }

        /// <summary>
        /// Position of the option in the poll
        /// </summary>
        [JsonProperty("position")] public int Position { get; set; }

        /// <summary>
        /// Number of votes for this option
        /// </summary>
        [JsonProperty("votes")] public int Votes { get; set; }
    }
}