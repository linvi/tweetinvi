using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Tweetinvi.Core.Interfaces.Streaminvi;

namespace Tweetinvi.Streams.Helpers
{
    /// <summary>
    /// List of methods to be used to Track keywords
    /// </summary>
    public class StreamTrackManager<T> : IStreamTrackManager<T>
    {
        protected bool _refreshTracking;
        protected readonly int _maxTracks;

        // Stores the entire track

        private readonly Dictionary<string, Action<T>> _tracks;
        public Dictionary<string, Action<T>> Tracks
        {
            get { return _tracks; }
        }

        // Stores the keywords included in a track
        protected readonly List<string[]> _tracksKeywords;

        public int TracksCount
        {
            get { return _tracks.Count; }
        }

        public int MaxTracks
        {
            get { return _maxTracks; }
        }

        public StreamTrackManager()
            : this(Int32.MaxValue)
        {
        }

        public StreamTrackManager(int maxTrack)
        {
            _maxTracks = maxTrack;

            _tracks = new Dictionary<string, Action<T>>();
            _tracksKeywords = new List<string[]>();
        }

        // Twitter API Tracking
        public void AddTrack(string track, Action<T> trackReceived = null)
        {
            if (_tracks.Count < MaxTracks)
            {
                string lowerTrack = track.ToLower();
                string[] trackSplit = lowerTrack.Split(' ');

                lock (this) // Not allowed to add multiple at the same time
                {
                    if (!_tracks.Keys.Contains(lowerTrack))
                    {
                        _tracks.Add(lowerTrack, trackReceived);
                        _tracksKeywords.Add(trackSplit);
                    }
                }

                _refreshTracking = true;
            }
        }

        public void RemoveTrack(string track)
        {
            string lowerTrack = track.ToLower();

            lock (this) // Not allowed to remove multiple at the same time
            {
                if (_tracks.Keys.Contains(lowerTrack))
                {
                    string[] trackSplit = lowerTrack.Split(' ');

                    // Do not use .ContainsSameObjectsAs for performances of Array vs IEnumerable
                    _tracksKeywords.RemoveAll(x => x.Length == trackSplit.Length &&
                                                  !x.Except(trackSplit).Any());
                    _tracks.Remove(lowerTrack);
                }
            }

            _refreshTracking = true;
        }

        public bool ContainsTrack(string track)
        {
            return _tracks.Keys.Contains(track.ToLower());
        }

        public void ClearTracks()
        {
            lock (this)
            {
                _tracks.Clear();
                _tracksKeywords.Clear();
                _refreshTracking = true;
            }
        }

        // Manual Tracking
        // private string[] _uniqueKeywordsArray;
        private HashSet<string> _uniqueKeywordsHashSet;
        private string[][] _tracksKeywordsArray;
        private Regex _matchingRegex;

        /// <summary>
        /// Creates Arrays of string that cache information for later comparisons
        /// This is required for performances improvement
        /// </summary>
        private void RefreshTracking()
        {
            // List of keywords associated with a track
            _tracksKeywordsArray = _tracksKeywords.ToArray();
            _uniqueKeywordsHashSet = new HashSet<string>();
            
            for (int i = 0; i < _tracksKeywordsArray.Length; ++i)
            {
                _uniqueKeywordsHashSet.UnionWith(_tracksKeywordsArray[i]);
            }

            var tracksContainsAtSymbol = _uniqueKeywordsHashSet.Any(x => x.StartsWith("@"));
            var tracksContainsDollarTag = _uniqueKeywordsHashSet.Any(x => x.StartsWith("$"));

            var regexBuilder = new StringBuilder(@"[\#");
            
            if (tracksContainsAtSymbol)
            {
                regexBuilder.Append("@");
            }

            if (tracksContainsDollarTag)
            {
                regexBuilder.Append(@"\$");
            }

            regexBuilder.Append(@"]\w+|\w+");

            _matchingRegex = new Regex(regexBuilder.ToString(), RegexOptions.IgnoreCase);
        }

        public bool Matches(string input)
        {
            lock (this)
            {
                return _matchingTracks(input).Any();
            }
        }

        public bool MatchesAll(string input)
        {
            lock (this)
            {
                return _matchingTracks(input).Count == _tracks.Count;
            }
        }

        private List<string> _matchingCharacters(string input)
        {
            // This behavior allows live refresh of the tracking
            // But reduces considerably the performances of the first test
            // First attempt ~= 10 x Later Attempts
            if (_refreshTracking)
            {
                RefreshTracking();
                _refreshTracking = false;
            }

            List<string> matchingKeywords = new List<string>();
            for (int i = 0; i < _uniqueKeywordsHashSet.Count; ++i)
            {
                if (input.Contains(_uniqueKeywordsHashSet.ElementAt(i)))
                {
                    matchingKeywords.Add(_uniqueKeywordsHashSet.ElementAt(i));
                }
            }

            List<string> result = new List<string>();
            for (int i = 0; i < _tracksKeywordsArray.Length; ++i)
            {
                bool trackIsMatching = true;
                for (int j = 0; j < _tracksKeywordsArray[i].Length && trackIsMatching; ++j)
                {
                    trackIsMatching = matchingKeywords.Contains(_tracksKeywordsArray[i][j]);
                }

                if (trackIsMatching)
                {
                    result.Add(_tracks.Keys.ElementAt(i));
                }
            }

            return result;
        }

        public List<string> GetMatchingCharacters(string input)
        {
            lock (this)
            {
                return _matchingCharacters(input);
            }
        }

        public List<string> GetMatchingTracks(string input)
        {
            lock (this)
            {
                return _matchingTracks(input).Select(x => x.Item1).ToList();
            }
        }

        private List<Tuple<string, Action<T>>> _matchingTracks(string input)
        {
            // Missing match of # for simple tracked keywords
            if (string.IsNullOrEmpty(input) || _tracks.Count == 0)
            {
                return new List<Tuple<string, Action<T>>>();
            }

            // This behavior allows live refresh of the tracking
            // But reduces considerably the performances of the first test
            if (_refreshTracking)
            {
                RefreshTracking();
                _refreshTracking = false;
            }

            var matchingKeywords = GetMatchingKeywords(input);

            var result = new List<Tuple<string, Action<T>>>();
            for (int i = 0; i < _tracksKeywordsArray.Length; ++i)
            {
                var isMatching = true;
                for (int j = 0; j < _tracksKeywordsArray[i].Length && isMatching; ++j)
                {
                    if (_tracksKeywordsArray[i][j][0] != '#' && _tracksKeywordsArray[i][j][0] != '$')
                    {
                        isMatching = matchingKeywords.Contains(_tracksKeywordsArray[i][j]) ||
                                     matchingKeywords.Contains(string.Format("#{0}", _tracksKeywordsArray[i][j])) ||
                                     matchingKeywords.Contains(string.Format("${0}", _tracksKeywordsArray[i][j]));
                    }
                    else
                    {
                        isMatching = matchingKeywords.Contains(_tracksKeywordsArray[i][j]);
                    }
                }

                if (isMatching)
                {
                    var keyword = _tracks.Keys.ElementAt(i);
                    var action = _tracks.ElementAt(i).Value;
                    result.Add(new Tuple<string, Action<T>>(keyword, action));
                }
            }

            return result;
        }

        private string[] GetMatchingKeywords(string input)
        {
            return _matchingRegex
                .Matches(input.ToLower())
                .OfType<Match>()
                .Where(match =>
                {
                    if (match.Value[0] == '#' || match.Value[0] == '$')
                    {
                        return _uniqueKeywordsHashSet.Contains(match.Value) ||
                               _uniqueKeywordsHashSet.Contains(match.Value.Substring(1, match.Value.Length - 1));
                    }

                    return _uniqueKeywordsHashSet.Contains(match.Value);
                })
                .Select(x => x.Value).ToArray();
        }
        
        public List<Tuple<string, Action<T>>> GetMatchingTracksAndActions(string input)
        {
            lock (this)
            {
                return _matchingTracks(input);
            }
        }
    }
}