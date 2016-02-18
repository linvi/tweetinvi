using System;
using System.Collections.Generic;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.Controllers.Messages;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Parameters;

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

        #region Get Messages Received
        [TestMethod]
        public void GetLatestMessagesReceived_ReturnsQueryExecutorDTOTransformedIntoModel()
        {
            var expectedResult = new List<IMessage> { A.Fake<IMessage>() };

            // Arrange
            var controller = CreateMessageController();
            var maximumMessages = new Random().Next();
            var expectedDTOResult = new List<IMessageDTO> { A.Fake<IMessageDTO>() };

            ArrangeQueryExecutorGetLatestMessagesReceived(maximumMessages, expectedDTOResult);
            ArrangeMessageFactoryGenerateMessages(expectedDTOResult, expectedResult);

            // Act
            var result = controller.GetLatestMessagesReceived(maximumMessages);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        private void ArrangeQueryExecutorGetLatestMessagesReceived(int maxMessages, IEnumerable<IMessageDTO> result)
        {
            _fakeMessageQueryExecutor
                .CallsTo(x => x.GetLatestMessagesReceived(maxMessages))
                .Returns(result);
        }
        #endregion

        #region Get Messages Sent
        [TestMethod]
        public void GetLatestMessagesSent_ReturnsQueryExecutorDTOTransformedIntoModel()
        {
            var expectedResult = new List<IMessage> { A.Fake<IMessage>() };

            // Arrange
            var controller = CreateMessageController();
            var maximumMessages = new Random().Next();
            var expectedDTOResult = new List<IMessageDTO> { A.Fake<IMessageDTO>() };

            ArrangeQueryExecutorGetLatestMessagesSent(maximumMessages, expectedDTOResult);
            ArrangeMessageFactoryGenerateMessages(expectedDTOResult, expectedResult);

            // Act
            var result = controller.GetLatestMessagesSent(maximumMessages);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        private void ArrangeQueryExecutorGetLatestMessagesSent(int maxMessages, IEnumerable<IMessageDTO> result)
        {
            _fakeMessageQueryExecutor
                .CallsTo(x => x.GetLatestMessagesSent(maxMessages))
                .Returns(result);
        }
        #endregion

        #region Publish Message

        [TestMethod]
        public void PublishMessage_MessageIsNull_ThrowArgumentException()
        {
            // Arrange
            var controller = CreateMessageController();

            // Act
            try
            {
                controller.PublishMessage((IMessage)null);

            }
            catch (ArgumentNullException)
            {
                return;
            }

            Assert.Fail("Argument Null Exception is expected");
        }
     
        [TestMethod]
        public void PublishMessage_TextAndUser_UserIsNull_ThrowArgumentException()
        {
            // Arrange
            var controller = CreateMessageController();
            var text = TestHelper.GenerateString();

            // Act - Assert
            try
            {
                controller.PublishMessage(text, (IUser)null);
            }
            catch (ArgumentNullException)
            {
                return;
            }

            Assert.Fail("Argument Null Exception is expected");
        }

        [TestMethod]
        public void PublishMessage_Parameter_IsNull_ThrowArgumentException()
        {
            // Arrange
            var controller = CreateMessageController();
            var text = TestHelper.GenerateString();

            // Act - Assert
            try
            {
                controller.PublishMessage(text, (IUser)null);
            }
            catch (ArgumentNullException)
            {
                return;
            }

            Assert.Fail("Argument Null Exception is expected");
        }

        [TestMethod]
        public void PublishMessage_WithMessage_ExecuteQuery()
        {
            // Arrange
            var controller = CreateMessageController();
            var message = A.Fake<IMessage>();
            message.CallsTo(x => x.Recipient).Returns(A.Fake<IUser>());
            message.CallsTo(x => x.Text).Returns(TestHelper.GenerateString());
            message.CallsTo(x => x.MessageDTO).Returns(A.Fake<IMessageDTO>());

            var parameter = new PublishMessageParameters(message);

            // Act
            controller.PublishMessage(parameter);

            // Assert
            _fakeMessageQueryExecutor
                .CallsTo(x => x.PublishMessage(A<IPublishMessageParameters>.That.Matches(p => p.Message == message.MessageDTO)))
                .MustHaveHappened();
        }

        [TestMethod]
        public void PublishMessage_WithMessageDTO_ExecuteQuery()
        {
            // Arrange
            var controller = CreateMessageController();
            var message = A.Fake<IMessageDTO>();
            message.CallsTo(x => x.Recipient).Returns(A.Fake<IUserDTO>());
            message.CallsTo(x => x.Text).Returns(TestHelper.GenerateString());

            var parameter = new PublishMessageParameters(message);

            // Act
            controller.PublishMessage(parameter);

            // Assert
            _fakeMessageQueryExecutor
                .CallsTo(x => x.PublishMessage(A<IPublishMessageParameters>.That.Matches(p => p.Message == message)))
                .MustHaveHappened();
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
        [ExpectedException(typeof(ArgumentException))]
        public void DestroyMessage_WithNullTweet_ThrowArgumentException()
        {
            // Arrange
            var controller = CreateMessageController();
            _fakeMessageQueryExecutor.CallsTo(x => x.DestroyMessage(A<IMessageDTO>.Ignored)).Returns(true);

            // Act
            controller.DestroyMessage((IMessage)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DestroyMessage_WithNullTweetDTO_ThrowArgumentException()
        {
            // Arrange
            var controller = CreateMessageController();
            var message = A.Fake<IMessage>();
            message.CallsTo(x => x.MessageDTO).Returns(null);

            _fakeMessageQueryExecutor.CallsTo(x => x.DestroyMessage(A<IMessageDTO>.Ignored)).Returns(true);

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
            var messageDTO = A.Fake<IMessageDTO>();

            var message = A.Fake<IMessage>();
            message.CallsTo(x => x.MessageDTO).Returns(messageDTO);

            _fakeMessageQueryExecutor.CallsTo(x => x.DestroyMessage(messageDTO)).Returns(result);

            // Act
            return controller.DestroyMessage(message);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public bool DestroyMessageDTO_WithNullMessageDTO_ThrowArgumentExcepton()
        {
            // Arrange
            var controller = CreateMessageController();
            _fakeMessageQueryExecutor.CallsTo(x => x.DestroyMessage(It.IsAny<IMessageDTO>())).Returns(true);

            // Act
            return controller.DestroyMessage((IMessageDTO)null);
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
            var messageDTO = A.Fake<IMessageDTO>();

            _fakeMessageQueryExecutor.CallsTo(x => x.DestroyMessage(messageDTO)).Returns(result);

            // Act
            return controller.DestroyMessage(messageDTO);
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

        private void ArrangeMessageFactoryGenerateMessage(IMessageDTO messageDTO, IMessage result)
        {
            _fakeMessageFactory
                .CallsTo(x => x.GenerateMessageFromMessageDTO(messageDTO))
                .Returns(result);
        }

        private void ArrangeMessageFactoryGenerateMessages(IEnumerable<IMessageDTO> messageDTOs, IEnumerable<IMessage> result)
        {
            _fakeMessageFactory
                .CallsTo(x => x.GenerateMessagesFromMessagesDTO(messageDTOs))
                .Returns(result);
        }

        public MessageController CreateMessageController()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}