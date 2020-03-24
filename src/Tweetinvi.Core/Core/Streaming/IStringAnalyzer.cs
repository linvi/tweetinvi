using System.Collections.Generic;

namespace Tweetinvi.Core.Streaming
{
    public interface ITrackStringAnalyzer
    {
        /// <summary>
        /// Verify that the input matches one requirement
        /// </summary>
        bool Matches(string input);

        /// <summary>
        /// Verify that the input matches all requirements
        /// </summary>
        bool MatchesAll(string input);

        /// <summary>
        /// Collection of chars[] (keywords) matched in the input.
        /// e.g. : 'linvi' matches 'hello linvi from tweetinvi'
        /// e.g. : 'linvi' matches 'hellolinvifromtweetinvi'
        /// </summary>
        List<string> GetMatchingCharacters(string input);

        /// <summary>
        /// Collection of tracked Keywords matched in the input
        /// e.g. : 'linvi' matches 'hello linvi from tweetinvi'
        /// e.g. : 'linvi' does not match 'hellolinvifromtweetinvi'
        /// </summary>
        List<string> GetMatchingTracks(string input);
    }
}