using System;
using System.Threading.Tasks;
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

        #region Publish Message

        [TestMethod]
        public async Task PublishMessage_WithCreateMessageDTO_ReturnsTwitterAccessor()
        {
            // Arrange
            var queryExecutor = CreateMessageQueryExecutor();
            var parameters = A.Fake<IPublishMessageParameters>();
            var reqDTO = A.Fake<ICreateMessageDTO>();
            var resDTO = A.Fake<ICreateMessageDTO>();
            var query = TestHelper.GenerateString();

            ArrangeQueryGeneratorPublishMessage(parameters, query, reqDTO);
            _fakeTwitterAccessor.ArrangeExecutePostQueryJsonBody(query, reqDTO, resDTO);

            // Act
            var result = await queryExecutor.PublishMessage(parameters);

            // Assert
            Assert.AreEqual(result, resDTO);
        }

        private void ArrangeQueryGeneratorPublishMessage(IPublishMessageParameters parameters, string query,
            ICreateMessageDTO createMessageDTO)
        {
            _fakeMessageQueryGenerator
                .CallsTo(x => x.GetPublishMessageQuery(parameters))
                .Returns(query);

            _fakeMessageQueryGenerator
                .CallsTo(x => x.GetPublishMessageBody(parameters))
                .Returns(createMessageDTO);
        }

        #endregion

        #region Destroy Message

        [TestMethod]
        public async Task DestroyMessage_WithMessageDTO_ReturnsTwitterAccessorResult()
        {
            // Arrange - Act
            var result1 = await DestroyMessage_WithMessageDTO_Returns(true);
            var result2 = await DestroyMessage_WithMessageDTO_Returns(false);

            // Assert
            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
        }

        private async Task<bool> DestroyMessage_WithMessageDTO_Returns(bool expectedResult)
        {
            // Arrange
            var queryExecutor = CreateMessageQueryExecutor();
            var eventDTO = A.Fake<IMessageEventDTO>();
            var query = Guid.NewGuid().ToString();

            ArrangeQueryGeneratorDestroyMessage(eventDTO, query);
            _fakeTwitterAccessor.ArrangeTryExecuteDELETEQuery(query, expectedResult);

            // Act
            return await queryExecutor.DestroyMessage(eventDTO);
        }

        private void ArrangeQueryGeneratorDestroyMessage(IMessageEventDTO messageEventDTO, string query)
        {
            _fakeMessageQueryGenerator
                .CallsTo(x => x.GetDestroyMessageQuery(messageEventDTO))
                .Returns(query);
        }

        [TestMethod]
        public async Task DestroyMessage_WithMessageId_ReturnsTwitterAccessorResult()
        {
            // Arrange - Act
            var result1 = await DestroyMessage_WithMessageId_Returns(true);
            var result2 = await DestroyMessage_WithMessageId_Returns(false);

            // Assert
            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
        }

        private async Task<bool> DestroyMessage_WithMessageId_Returns(bool expectedResult)
        {
            // Arrange
            var queryExecutor = CreateMessageQueryExecutor();
            var messageId = new Random().Next();
            var query = Guid.NewGuid().ToString();

            ArrangeQueryGeneratorDestroyMessage(messageId, query);
            _fakeTwitterAccessor.ArrangeTryExecuteDELETEQuery(query, expectedResult);

            // Act
            return await queryExecutor.DestroyMessage(messageId);
        }

        private void ArrangeQueryGeneratorDestroyMessage(long userId, string query)
        {
            _fakeMessageQueryGenerator
                .CallsTo(x => x.GetDestroyMessageQuery(userId))
                .Returns(query);
        }

        #endregion

        private MessageQueryExecutor CreateMessageQueryExecutor()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}