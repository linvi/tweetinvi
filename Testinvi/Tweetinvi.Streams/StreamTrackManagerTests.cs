using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.Streams.Helpers;

namespace Testinvi.Tweetinvi.Streams
{
    [TestClass]
    public class StreamTrackManagerTests
    {
        private FakeClassBuilder<StreamTrackManager<object>> _fakeBuilder;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<StreamTrackManager<object>>();
        }

        [TestMethod]
        public void Track_SimpleKeyword_IsMatchedCorrectly()
        {
            // Arrange
            var trackManager = CreateStreamTrackManager();
            trackManager.AddTrack("PilOupe");

            // Act
            var matchingTracks = trackManager.GetMatchingTracks("plop there is a piloupE!");

            // Assert
            Assert.IsTrue(matchingTracks.Count == 1 && matchingTracks[0] == "piloupe");
        }

        [TestMethod]
        public void Track_WithHashTag_IsMatchedCorrectly()
        {
            // Arrange
            var trackManager = CreateStreamTrackManager();
            trackManager.AddTrack("#HashTag");

            // Act
            var matchingTracks = trackManager.GetMatchingTracks("plop there is a #hasHtag!");

            // Assert
            Assert.IsTrue(matchingTracks.Count == 1 && matchingTracks[0] == "#hashtag");
        }

        [TestMethod]
        public void Track_DollarsTag_IsMatchedCorrectly()
        {
            // Arrange
            var trackManager = CreateStreamTrackManager();
            trackManager.AddTrack("$DollarsTag");

            // Act
            var matchingTracks = trackManager.GetMatchingTracks("plop there is a $doLlarstag!");

            // Assert
            Assert.IsTrue(matchingTracks.Count == 1 && matchingTracks[0] == "$dollarstag");
        }

        [TestMethod]
        public void Track_WithMention_IsMatchedCorrectly()
        {
            // Arrange
            var trackManager = CreateStreamTrackManager();
            trackManager.AddTrack("@Mention");

            // Act
            var matchingTracks = trackManager.GetMatchingTracks("plop there is a @mEntion!");

            // Assert
            Assert.IsTrue(matchingTracks.Count == 1 && matchingTracks[0] == "@mention");
        }

        public StreamTrackManager<object> CreateStreamTrackManager()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}
