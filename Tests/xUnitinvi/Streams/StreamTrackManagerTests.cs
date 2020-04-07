using Tweetinvi.Streams.Helpers;
using Xunit;
using xUnitinvi.TestHelpers;

namespace xUnitinvi.Streams
{
    public class StreamTrackManagerTests
    {
        private readonly FakeClassBuilder<StreamTrackManager<object>> _fakeBuilder;

        public StreamTrackManagerTests()
        {
            _fakeBuilder = new FakeClassBuilder<StreamTrackManager<object>>();
        }

        [Fact]
        public void Track_SimpleKeyword_IsMatchedCorrectly()
        {
            // Arrange
            var trackManager = CreateStreamTrackManager();
            trackManager.AddTrack("PilOupe");

            // Act
            var matchingTracks = trackManager.GetMatchingTracks("plop there is a piloupE!");

            // Assert
            Assert.True(matchingTracks.Count == 1 && matchingTracks[0] == "piloupe");
        }

        [Fact]
        public void Track_WithHashTag_IsMatchedCorrectly()
        {
            // Arrange
            var trackManager = CreateStreamTrackManager();
            trackManager.AddTrack("#HashTag");

            // Act
            var matchingTracks = trackManager.GetMatchingTracks("plop there is a #hasHtag!");

            // Assert
            Assert.True(matchingTracks.Count == 1 && matchingTracks[0] == "#hashtag");
        }

        [Fact]
        public void Track_DollarsTag_IsMatchedCorrectly()
        {
            // Arrange
            var trackManager = CreateStreamTrackManager();
            trackManager.AddTrack("$DollarsTag");

            // Act
            var matchingTracks = trackManager.GetMatchingTracks("plop there is a $doLlarstag!");

            // Assert
            Assert.True(matchingTracks.Count == 1 && matchingTracks[0] == "$dollarstag");
        }

        [Fact]
        public void Track_WithMention_IsMatchedCorrectly()
        {
            // Arrange
            var trackManager = CreateStreamTrackManager();
            trackManager.AddTrack("@Mention");

            // Act
            var matchingTracks = trackManager.GetMatchingTracks("plop there is a @mEntion!");

            // Assert
            Assert.True(matchingTracks.Count == 1 && matchingTracks[0] == "@mention");
        }

        public StreamTrackManager<object> CreateStreamTrackManager()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}
