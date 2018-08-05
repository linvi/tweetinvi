using System;
using System.Collections.Generic;
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
        public void GetLatestMessages_ReturnsQueryExecutorDTOTransformedIntoModel()
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
            var result = controller.GetLatestMessages(count);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        private void ArrangeQueryExecutorGetLatestMessages(IGetMessagesDTO result)
        {
            _fakeMessageQueryExecutor
                .CallsTo(x => x.GetLatestMessages(A<IGetMessagesParameters>.Ignored))
                .Returns(result);
        }
        #endregion

        #region Publish Message

        [TestMethod]
        public void PublishMessage_Parameter_IsNull_ThrowArgumentException()
        {
            // Arrange
            var controller = CreateMessageController();

            // Act - Assert
            try
            {
                controller.PublishMessage(null);
            }
            catch (ArgumentNullException)
            {
                return;
            }

            Assert.Fail("Argument Null Exception is expected");
        }

        [TestMethod]
        public void PublishMessage_WithParameter_ExecuteQuery()
        {
            // Arrange
            var controller = CreateMessageController();
            var parameter = new PublishMessageParameters(TestHelper.GenerateString(), TestHelper.GenerateRandomInt());

            // Act
            controller.PublishMessage(parameter);

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
            _fakeMessageQueryExecutor.CallsTo(x => x.DestroyMessage(A<IEventDTO>.Ignored)).Returns(true);

            // Act
            controller.DestroyMessage((IMessage)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DestroyMessage_WithNullEventDTO_ThrowArgumentException()
        {
            // Arrange
            var controller = CreateMessageController();
            var message = A.Fake<IMessage>();
            message.CallsTo(x => x.EventDTO).Returns(null);

            _fakeMessageQueryExecutor.CallsTo(x => x.DestroyMessage(A<IEventDTO>.Ignored)).Returns(true);

            // Act
            controller.DestroyMessage(message);
        }

        [TestMethod]
        public void DestroyMessage_ReturnsQueryExecutorResult()
        {
            // Arrange - Act
            var result1 = DestroyMessage_QueryExecutorReturns(true);
            var result2 = DestroyMessage_QueryExecutorReturns(false);

            // Assert
            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
        }

        private bool DestroyMessage_QueryExecutorReturns(bool result)
        {
            // Arrange
            var controller = CreateMessageController();
            var eventDTO = A.Fake<IEventDTO>();

            var message = A.Fake<IMessage>();
            message.CallsTo(x => x.EventDTO).Returns(eventDTO);

            _fakeMessageQueryExecutor.CallsTo(x => x.DestroyMessage(eventDTO)).Returns(result);

            // Act
            return controller.DestroyMessage(message);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DestroyMessageDTO_WithNullEventDTO_ThrowArgumentExcepton()
        {
            // Arrange
            var controller = CreateMessageController();
            _fakeMessageQueryExecutor.CallsTo(x => x.DestroyMessage(It.IsAny<IEventDTO>())).Returns(true);

            // Act
            controller.DestroyMessage((IEventDTO) null);
        }

        [TestMethod]
        public void DestroyMessageDTO_ReturnsQueryExecutorResult()
        {
            // Arrange - Act
            var result1 = DestroyMessageDTO_QueryExecutorReturns(true);
            var result2 = DestroyMessageDTO_QueryExecutorReturns(false);

            // Assert
            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
        }

        private bool DestroyMessageDTO_QueryExecutorReturns(bool result)
        {
            // Arrange
            var controller = CreateMessageController();
            var eventDTO = A.Fake<IEventDTO>();

            _fakeMessageQueryExecutor.CallsTo(x => x.DestroyMessage(eventDTO)).Returns(result);

            // Act
            return controller.DestroyMessage(eventDTO);
        }

        [TestMethod]
        public void DestroyMessageId_ReturnsQueryExecutorResult()
        {
            // Arrange - Act
            var result1 = DestroyMessageId_QueryExecutorReturns(true);
            var result2 = DestroyMessageId_QueryExecutorReturns(false);

            // Assert
            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
        }

        private bool DestroyMessageId_QueryExecutorReturns(bool result)
        {
            // Arrange
            var controller = CreateMessageController();
            var messageId = TestHelper.GenerateRandomLong();

            _fakeMessageQueryExecutor
                .CallsTo(x => x.DestroyMessage(messageId))
                .Returns(result);

            // Act
            return controller.DestroyMessage(messageId);
        }

        #endregion

        private void ArrangeMessageFactoryGenerateMessage(IGetMessageDTO getMessageDTO, IMessage result)
        {
            _fakeMessageFactory
                .CallsTo(x => x.GenerateMessageFromGetMessageDTO(getMessageDTO))
                .Returns(result);
        }

        private void ArrangeMessageFactoryGenerateMessages(IGetMessagesDTO getMessagesDTO, IEnumerable<IMessage> result)
        {
            _fakeMessageFactory
                .CallsTo(x => x.GenerateMessageFromGetMessagesDTO(getMessagesDTO))
                .Returns(result);
        }

        public MessageController CreateMessageController()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}