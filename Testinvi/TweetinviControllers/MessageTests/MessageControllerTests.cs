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
        [ExpectedException(typeof(ArgumentException))]
        public void PublishMessage_MessageIsNull_ThrowArgumentException(bool messageDTOExists, IMessage result)
        {
            var message = A.Fake<IMessage>();
            var sourceMessageDTO = messageDTOExists ? A.Fake<IMessageDTO>() : null;
            message.CallsTo(x => x.MessageDTO).Returns(sourceMessageDTO);

            // Arrange
            var controller = CreateMessageController();
            var resultMessageDTO = A.Fake<IMessageDTO>();

            ArrangeQueryExecutorPublishMessageWithMessageDTO(It.IsAny<IMessageDTO>(), resultMessageDTO);
            ArrangeMessageFactoryGenerateMessage(resultMessageDTO, result);

            // Act
            controller.PublishMessage((IMessage)null);
        }

        [TestMethod]
        public void PublishMessage_Message_QueryExecutorDTOTransformedIntoModel()
        {
            var expectedMessage1 = A.Fake<IMessage>();
            var expectedMessage2 = A.Fake<IMessage>();

            // Arrange - Act
            var message1 = PublishMessage_Message_QueryExecutorDTOTransformedIntoModel(true, expectedMessage1);
            var message2 = PublishMessage_Message_QueryExecutorDTOTransformedIntoModel(false, expectedMessage2);

            // Assert
            Assert.AreEqual(message1, expectedMessage1);
            Assert.AreEqual(message2, expectedMessage2);
        }

        private IMessage PublishMessage_Message_QueryExecutorDTOTransformedIntoModel(bool messageDTOExists, IMessage result)
        {
            var message = A.Fake<IMessage>();
            var sourceMessageDTO = messageDTOExists ? A.Fake<IMessageDTO>() : null;
            message.CallsTo(x => x.MessageDTO).Returns(sourceMessageDTO);

            // Arrange
            var controller = CreateMessageController();
            var resultMessageDTO = A.Fake<IMessageDTO>();

            ArrangeQueryExecutorPublishMessageWithMessageDTO(sourceMessageDTO, resultMessageDTO);
            ArrangeMessageFactoryGenerateMessage(resultMessageDTO, result);

            // Act
            return controller.PublishMessage(message);
        }

        [TestMethod]
        public void PublishMessage_MessageDTO_QueryExecutorDTOTransformedIntoModel()
        {
            var expectedMessage1 = A.Fake<IMessage>();
            var expectedMessage2 = A.Fake<IMessage>();

            // Arrange - Act
            var message1 = PublishMessage_MessageDTO_QueryExecutorDTOTransformedIntoModel(true, expectedMessage1);
            var message2 = PublishMessage_MessageDTO_QueryExecutorDTOTransformedIntoModel(false, expectedMessage2);

            // Assert
            Assert.AreEqual(message1, expectedMessage1);
            Assert.AreEqual(message2, expectedMessage2);
        }

        private IMessage PublishMessage_MessageDTO_QueryExecutorDTOTransformedIntoModel(bool messageExists, IMessage result)
        {
            var sourceMessageDTO = messageExists ? A.Fake<IMessageDTO>() : null;
            var resultMessageDTO = A.Fake<IMessageDTO>();

            // Arrange
            var controller = CreateMessageController();

            ArrangeQueryExecutorPublishMessageWithMessageDTO(sourceMessageDTO, resultMessageDTO);
            ArrangeMessageFactoryGenerateMessage(resultMessageDTO, result);

            // Act
            return controller.PublishMessage(sourceMessageDTO);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PublishMessage_TextAndUser_UserIsNull_ThrowArgumentException()
        {
            // Arrange
            var controller = CreateMessageController();
            var text = TestHelper.GenerateString();

            ArrangeQueryExecutorPublishMessageWithTextAndUserDTO(It.IsAny<string>(), It.IsAny<IUserDTO>(), A.Fake<IMessageDTO>());

            // Act
            controller.PublishMessage(text, (IUser)null);
        }

        [TestMethod]
        public void PublishMessage_TextAndUser_QueryExecutorDTOTransformedIntoModel()
        {
            var resultMessage1 = A.Fake<IMessage>();
            var resultMessage2 = A.Fake<IMessage>();
            var resultMessage3 = A.Fake<IMessage>();
            var resultMessage4 = A.Fake<IMessage>();

            // Arrange - Act
            var message1 = PublishMessage_TextAndUser_QueryExecutorDTOTransformedIntoModel(false, false, resultMessage1);
            var message2 = PublishMessage_TextAndUser_QueryExecutorDTOTransformedIntoModel(false, true, resultMessage2);

            var message3 = PublishMessage_TextAndUser_QueryExecutorDTOTransformedIntoModel(true, false, resultMessage3);
            var message4 = PublishMessage_TextAndUser_QueryExecutorDTOTransformedIntoModel(true, true, resultMessage4);

            // Assert
            Assert.AreEqual(message1, resultMessage1);
            Assert.AreEqual(message2, resultMessage2);
            Assert.AreEqual(message3, resultMessage3);
            Assert.AreEqual(message4, resultMessage4);
        }

        private IMessage PublishMessage_TextAndUser_QueryExecutorDTOTransformedIntoModel(bool textExists, bool recipientDTOExists, IMessage result)
        {
            string text = textExists ? Guid.NewGuid().ToString() : null;
            var recipient = A.Fake<IUser>();
            var recipientDTO = recipientDTOExists ? A.Fake<IUserDTO>() : null;
            recipient.CallsTo(x => x.UserDTO).Returns(recipientDTO);

            // Arrange
            var controller = CreateMessageController();
            var messageDTO = A.Fake<IMessageDTO>();

            ArrangeQueryExecutorPublishMessageWithTextAndUserDTO(text, recipientDTO, messageDTO);
            ArrangeMessageFactoryGenerateMessage(messageDTO, result);

            // Act
            return controller.PublishMessage(text, recipient);
        }

        [TestMethod]
        public void PublishMessage_TextAndUserDTO_QueryExecutorDTOTransformedIntoModel()
        {
            var expectedMessage1 = A.Fake<IMessage>();
            var expectedMessage2 = A.Fake<IMessage>();
            var expectedMessage3 = A.Fake<IMessage>();
            var expectedMessage4 = A.Fake<IMessage>();

            // Arrange - Act
            var message1 = PublishMessage_TextAndUserDTO_QueryExecutorDTOTransformedIntoModel(true, true, expectedMessage1);
            var message2 = PublishMessage_TextAndUserDTO_QueryExecutorDTOTransformedIntoModel(true, false, expectedMessage2);
            var message3 = PublishMessage_TextAndUserDTO_QueryExecutorDTOTransformedIntoModel(false, true, expectedMessage3);
            var message4 = PublishMessage_TextAndUserDTO_QueryExecutorDTOTransformedIntoModel(false, false, expectedMessage4);

            // Assert
            Assert.AreEqual(message1, expectedMessage1);
            Assert.AreEqual(message2, expectedMessage2);
            Assert.AreEqual(message3, expectedMessage3);
            Assert.AreEqual(message4, expectedMessage4);
        }

        private IMessage PublishMessage_TextAndUserDTO_QueryExecutorDTOTransformedIntoModel(bool textExists, bool recipientExists, IMessage result)
        {
            string text = textExists ? Guid.NewGuid().ToString() : null;
            var recipient = recipientExists ? A.Fake<IUserDTO>() : null;

            // Arrange
            var controller = CreateMessageController();
            var messageDTO = A.Fake<IMessageDTO>();

            ArrangeQueryExecutorPublishMessageWithTextAndUserDTO(text, recipient, messageDTO);
            ArrangeMessageFactoryGenerateMessage(messageDTO, result);

            // Act
            return controller.PublishMessage(text, recipient);
        }

        [TestMethod]
        public void PublishMessage_TextAndUserId_QueryExecutorDTOTransformedIntoModel()
        {
            var expectedMessage1 = A.Fake<IMessage>();
            var expectedMessage2 = A.Fake<IMessage>();

            // Arrange - Act
            var message1 = PublishMessage_TextAndUserId_QueryExecutorDTOTransformedIntoModel(true, expectedMessage1);
            var message2 = PublishMessage_TextAndUserId_QueryExecutorDTOTransformedIntoModel(false, expectedMessage2);

            // Assert
            Assert.AreEqual(message1, expectedMessage1);
            Assert.AreEqual(message2, expectedMessage2);
        }

        private IMessage PublishMessage_TextAndUserId_QueryExecutorDTOTransformedIntoModel(bool textExists, IMessage result)
        {
            string text = textExists ? Guid.NewGuid().ToString() : null;
            var recipientId = TestHelper.GenerateRandomLong();

            // Arrange
            var controller = CreateMessageController();
            var messageDTO = A.Fake<IMessageDTO>();

            ArrangeQueryExecutorPublishMessageWithTextAndUserId(text, recipientId, messageDTO);
            ArrangeMessageFactoryGenerateMessage(messageDTO, result);

            // Act
            return controller.PublishMessage(text, recipientId);
        }

        [TestMethod]
        public void PublishMessage_TextAndScreenName_QueryExecutorDTOTransformedIntoModel()
        {
            var expectedMessage1 = A.Fake<IMessage>();
            var expectedMessage2 = A.Fake<IMessage>();
            var expectedMessage3 = A.Fake<IMessage>();
            var expectedMessage4 = A.Fake<IMessage>();

            // Arrange - Act
            var message1 = PublishMessage_TextAndScreenName_QueryExecutorDTOTransformedIntoModel(true, true, expectedMessage1);
            var message2 = PublishMessage_TextAndScreenName_QueryExecutorDTOTransformedIntoModel(true, false, expectedMessage2);
            var message3 = PublishMessage_TextAndScreenName_QueryExecutorDTOTransformedIntoModel(false, true, expectedMessage3);
            var message4 = PublishMessage_TextAndScreenName_QueryExecutorDTOTransformedIntoModel(false, false, expectedMessage4);

            // Assert
            Assert.AreEqual(message1, expectedMessage1);
            Assert.AreEqual(message2, expectedMessage2);
            Assert.AreEqual(message3, expectedMessage3);
            Assert.AreEqual(message4, expectedMessage4);
        }

        private IMessage PublishMessage_TextAndScreenName_QueryExecutorDTOTransformedIntoModel(bool textExists, bool recipientExists, IMessage result)
        {
            string text = textExists ? Guid.NewGuid().ToString() : null;
            string recipient = recipientExists ? Guid.NewGuid().ToString() : null;

            // Arrange
            var controller = CreateMessageController();
            var messageDTO = A.Fake<IMessageDTO>();

            ArrangeQueryExecutorPublishMessageWithTextAndScreenName(text, recipient, messageDTO);
            ArrangeMessageFactoryGenerateMessage(messageDTO, result);

            // Act
            return controller.PublishMessage(text, recipient);
        }

        private void ArrangeQueryExecutorPublishMessageWithMessageDTO(IMessageDTO sourceMessage, IMessageDTO result)
        {
            _fakeMessageQueryExecutor
                .CallsTo(x => x.PublishMessage(sourceMessage))
                .Returns(result);
        }

        private void ArrangeQueryExecutorPublishMessageWithTextAndUserDTO(string text, IUserDTO recipient, IMessageDTO result)
        {
            _fakeMessageQueryExecutor
                .CallsTo(x => x.PublishMessage(text, recipient))
                .Returns(result);
        }

        private void ArrangeQueryExecutorPublishMessageWithTextAndUserId(string text, long recipientId, IMessageDTO result)
        {
            _fakeMessageQueryExecutor
                .CallsTo(x => x.PublishMessage(text, recipientId))
                .Returns(result);
        }

        private void ArrangeQueryExecutorPublishMessageWithTextAndScreenName(string text, string recipient, IMessageDTO result)
        {
            _fakeMessageQueryExecutor
                .CallsTo(x => x.PublishMessage(text, recipient))
                .Returns(result);
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