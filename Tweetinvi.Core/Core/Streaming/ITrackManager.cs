using System;
using System.Collections.Generic;

namespace Tweetinvi.Core.Streaming
{
    /// <summary>
    /// Methods allowing to manage Track keywords
    /// </summary>
    public interface ITrackManager<T>
    {
        /// <summary>
        /// Gets the current number of Tracks
        /// </summary>
        int TracksCount { get; }

        /// <summary>
        /// Get the maximum number of Tracks you can add
        /// </summary>
        int MaxTracks { get; }

        /// <summary>
        /// List of tracks currently analyzed
        /// </summary>
        Dictionary<string, Action<T>> Tracks { get; }

        /// <summary>
        /// Add a keyword/sentence to Track
        /// </summary>
        /// <param name="track">Keyword to track</param>
        /// <param name="trackReceived">Event to call when this track keyword</param>
        void AddTrack(string track, Action<T> trackReceived = null);

        /// <summary>
        /// Remove a keyword/sentence that was tracked
        /// </summary>
        void RemoveTrack(string track);

        /// <summary>
        /// Tells whether a track is already existing (case insensitive)
        /// </summary>
        bool ContainsTrack(string track);

        /// <summary>
        /// Remove all tracked keywords
        /// </summary>
        void ClearTracks();
    }
}