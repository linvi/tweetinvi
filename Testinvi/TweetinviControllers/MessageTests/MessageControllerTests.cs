using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.Controllers.Messages;
using Tweetinvi.Core.Factories;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Testinvi.TweetinviControllers.MessageTests
{
    [TestClass]
    public class MessageControllerTests
    {
        private FakeClassBuilder<MessageController> _fakeBuilder;
        private Fake<IMessageQueryExecutor> _fakeMessageQueryExecutor;
        private Fake<IMessageFactory> _fakeMessageFactory;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<MessageController>();
            _fakeMessageQueryExecutor = _fakeBuilder.GetFake<IMessageQueryExecutor>();
            _fakeMessageFactory = _fakeBuilder.GetFake<IMessageFactory>();
        }

        #region Get Messages
        [TestMethod]
        public async Task GetLatestMessages_ReturnsQueryExecutorDTOTransformedIntoModel()
        {
            var expectedResult = new List<IMessage> { A.Fake<IMessage>() };

            // Arrange
            var controller = CreateMessageController();
            var count = new Random().Next();
            var expectedDTOResult = A.Fake<IGetMessagesDTO>();
            expectedDTOResult.CallsTo(x => x.NextCursor).Returns(null);

            ArrangeQueryExecutorGetLatestMessages(expectedDTOResult);
            ArrangeMessageFactoryGenerateMessages(expectedDTOResult, expectedResult);

            // Act
            var cursorResult = await controller.GetLatestMessages(count);

            // Assert
            Assert.AreEqual(cursorResult.Result, expectedResult);
        }

        private void ArrangeQueryExecutorGetLatestMessages(IGetMessagesDTO result)
        {
            _fakeMessageQueryExecutor
                .CallsTo(x => x.GetLatestMessages(A<IGetMessagesParameters>.Ignored))
                .ReturnsLazily(() => result);
        }
        #endregion

        #region Publish Message

        [TestMethod]
        public async Task PublishMessage_Parameter_IsNull_ThrowArgumentException()
        {
            // Arrange
            var controller = CreateMessageController();

            // Act - Assert
            try
            {
                await controller.PublishMessage(null);
            }
            catch (ArgumentNullException)
            {
                return;
            }

            Assert.Fail("Argument Null Exception is expected");
        }

        [TestMethod]
        public async Task PublishMessage_WithParameter_ExecuteQuery()
        {
            // Arrange
            var controller = CreateMessageController();
            var parameter = new PublishMessageParameters(TestHelper.GenerateString(), TestHelper.GenerateRandomInt());

            // Act
            await controller.PublishMessage(parameter);

            // Assert
            _fakeMessageQueryExecutor.CallsTo(x => x.PublishMessage(parameter)).MustHaveHappened();
        }

        #endregion

        #region Destroy Message

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DestroyMessage_NullMessage_ThrowArgumentException()
        {
            // Arrange
            var controller = CreateMessageController();
            _fakeMessageQueryExecutor.CallsTo(x => x.DestroyMessage(A<IMessageEventDTO>.Ignored)).Returns(true);

            // Act
            controller.DestroyMessage((IMessage)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task DestroyMessage_WithNullEventDTO_ThrowArgumentException()
        {
            // Arrange
            var controller = CreateMessageController();
            var message = A.Fake<IMessage>();
            message.CallsTo(x => x.MessageEventDTO).Returns(null);

            _fakeMessageQueryExecutor.CallsTo(x => x.DestroyMessage(A<IMessageEventDTO>.Ignored)).Returns(true);

            // Act
            await controller.DestroyMessage(message);
        }

        [TestMethod]
        public async Task DestroyMessage_ReturnsQueryExecutorResult()
        {
            // Arrange - Act
            var result1 = await DestroyMessage_QueryExecutorReturns(true);
            var result2 = await DestroyMessage_QueryExecutorReturns(false);

            // Assert
            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
        }

        private async Task<bool> DestroyMessage_QueryExecutorReturns(bool result)
        {
            // Arrange
            var controller = CreateMessageController();
            var eventDTO = A.Fake<IMessageEventDTO>();

            var message = A.Fake<IMessage>();
            message.CallsTo(x => x.MessageEventDTO).Returns(eventDTO);

            _fakeMessageQueryExecutor.CallsTo(x => x.DestroyMessage(eventDTO)).Returns(result);

            // Act
            return await controller.DestroyMessage(message);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task DestroyMessageDTO_WithNullEventDTO_ThrowArgumentException()
        {
            // Arrange
            var controller = CreateMessageController();
            _fakeMessageQueryExecutor.CallsTo(x => x.DestroyMessage(It.IsAny<IMessageEventDTO>())).Returns(true);

            // Act
            await controller.DestroyMessage((IMessageEventDTO) null);
        }

        [TestMethod]
        public async Task DestroyMessageDTO_ReturnsQueryExecutorResult()
        {
            // Arrange - Act
            var result1 = await DestroyMessageDTO_QueryExecutorReturns(true);
            var result2 = await DestroyMessageDTO_QueryExecutorReturns(false);

            // Assert
            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
        }

        private async Task<bool> DestroyMessageDTO_QueryExecutorReturns(bool result)
        {
            // Arrange
            var controller = CreateMessageController();
            var eventDTO = A.Fake<IMessageEventDTO>();

            _fakeMessageQueryExecutor.CallsTo(x => x.DestroyMessage(eventDTO)).Returns(result);

            // Act
            return await controller.DestroyMessage(eventDTO);
        }

        [TestMethod]
        public async Task DestroyMessageId_ReturnsQueryExecutorResult()
        {
            // Arrange - Act
            var result1 = await DestroyMessageId_QueryExecutorReturns(true);
            var result2 = await DestroyMessageId_QueryExecutorReturns(false);

            // Assert
            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
        }

        private async Task<bool> DestroyMessageId_QueryExecutorReturns(bool result)
        {
            // Arrange
            var controller = CreateMessageController();
            var messageId = TestHelper.GenerateRandomLong();

            _fakeMessageQueryExecutor
                .CallsTo(x => x.DestroyMessage(messageId))
                .Returns(result);

            // Act
            return await controller.DestroyMessage(messageId);
        }

        #endregion

        private void ArrangeMessageFactoryGenerateMessages(IGetMessagesDTO getMessagesDTO, IEnumerable<IMessage> result)
        {
            _fakeMessageFactory
                .CallsTo(x => x.GenerateMessageFromGetMessagesDTO(getMessagesDTO))
                .Returns(result);
        }

        private MessageController CreateMessageController()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}