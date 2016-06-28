using System;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi.Controllers.Messages;
using Tweetinvi.Core.Web;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Testinvi.TweetinviControllers.MessageTests
{
    [TestClass]
    public class MessageQueryExecutorTests
    {
        private FakeClassBuilder<MessageQueryExecutor> _fakeBuilder;
        private Fake<IMessageQueryGenerator> _fakeMessageQueryGenerator;
        private Fake<ITwitterAccessor> _fakeTwitterAccessor;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<MessageQueryExecutor>();
            _fakeMessageQueryGenerator = _fakeBuilder.GetFake<IMessageQueryGenerator>();
            _fakeTwitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>();
        }

        #region GetLatestMessagesReceived
        
        private void ArrangeQueryGeneratorGetLatestMessagesReceived(int maximumMessages, string query)
        {
            _fakeMessageQueryGenerator
                .CallsTo(x => x.GetLatestMessagesReceivedQuery(A<IMessagesReceivedParameters>.That.Matches(p => p.MaximumNumberOfMessagesToRetrieve == maximumMessages)))
                .Returns(query);
        }

        #endregion

        #region GetLatestMessagesSent

        private void ArrangeQueryGeneratorGetLatestMessagesSent(int maximumMessages, string query)
        {
            _fakeMessageQueryGenerator
                .CallsTo(x => x.GetLatestMessagesSentQuery(A<IMessagesSentParameters>.That.Matches(p => p.MaximumNumberOfMessagesToRetrieve == maximumMessages)))
                .Returns(query);
        }

        #endregion

        #region Publish Message

        [TestMethod]
        public void PublishMessage_WithMessageDTO_ReturnsTwitterAccessor()
        {
            // Arrange
            var queryExecutor = CreateMessageQueryExecutor();
            var parameters = A.Fake<IPublishMessageParameters>();
            var resultMessageDTO = A.Fake<IMessageDTO>();
            var query = TestHelper.GenerateString();

            ArrangeQueryGeneratorPublishMessage(parameters, query);
            _fakeTwitterAccessor.ArrangeExecutePOSTQuery(query, resultMessageDTO);

            // Act
            var result = queryExecutor.PublishMessage(parameters);

            // Assert
            Assert.AreEqual(result, resultMessageDTO);
        }

        private void ArrangeQueryGeneratorPublishMessage(IPublishMessageParameters parameters, string query)
        {
            _fakeMessageQueryGenerator
                .CallsTo(x => x.GetPublishMessageQuery(parameters))
                .Returns(query);
        }

        #endregion

        #region Destroy Message

        [TestMethod]
        public void DestroyMessage_WithMessageDTO_ReturnsTwitterAccessorResult()
        {
            // Arrange - Act
            var result1 = DestroyMessage_WithMessageDTO_Returns(true);
            var result2 = DestroyMessage_WithMessageDTO_Returns(false);

            // Assert
            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
        }

        public bool DestroyMessage_WithMessageDTO_Returns(bool expectedResult)
        {
            // Arrange
            var queryExecutor = CreateMessageQueryExecutor();
            var messageDTO = A.Fake<IMessageDTO>();
            var query = Guid.NewGuid().ToString();

            ArrangeQueryGeneratorDestroyMessage(messageDTO, query);
            _fakeTwitterAccessor.ArrangeTryExecutePOSTQuery(query, expectedResult);

            // Act
            return queryExecutor.DestroyMessage(messageDTO);
        }

        private void ArrangeQueryGeneratorDestroyMessage(IMessageDTO messageDTO, string query)
        {
            _fakeMessageQueryGenerator
                .CallsTo(x => x.GetDestroyMessageQuery(messageDTO))
                .Returns(query);
        }

        [TestMethod]
        public void DestroyMessage_WithMessageId_ReturnsTwitterAccessorResult()
        {
            // Arrange - Act
            var result1 = DestroyMessage_WithMessageId_Returns(true);
            var result2 = DestroyMessage_WithMessageId_Returns(false);

            // Assert
            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
        }

        public bool DestroyMessage_WithMessageId_Returns(bool expectedResult)
        {
            // Arrange
            var queryExecutor = CreateMessageQueryExecutor();
            var messageId = new Random().Next();
            var query = Guid.NewGuid().ToString();

            ArrangeQueryGeneratorDestroyMessage(messageId, query);
            _fakeTwitterAccessor.ArrangeTryExecutePOSTQuery(query, expectedResult);

            // Act
            return queryExecutor.DestroyMessage(messageId);
        }

        private void ArrangeQueryGeneratorDestroyMessage(long userId, string query)
        {
            _fakeMessageQueryGenerator
                .CallsTo(x => x.GetDestroyMessageQuery(userId))
                .Returns(query);
        }

        #endregion

        public MessageQueryExecutor CreateMessageQueryExecutor()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}