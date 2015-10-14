using System;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi.Controllers.Messages;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO;

namespace Testinvi.TweetinviControllers.MessageTests
{
    [TestClass]
    public class MessageJsonControllerTests
    {
        private FakeClassBuilder<MessageJsonController> _fakeBuilder;
        private Fake<IMessageQueryGenerator> _fakeMessageQueryGenerator;
        private Fake<ITwitterAccessor> _fakeTwitterAccessor;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<MessageJsonController>();
            _fakeMessageQueryGenerator = _fakeBuilder.GetFake<IMessageQueryGenerator>();
            _fakeTwitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>();
        }

        #region GetLatestMessagesReceived

        [TestMethod]
        public void GetLatestMessagesReceived_ReturnsTwitterAccessorResult()
        {
            // Arrange
            var jsonController = CreateMessageJsonController();
            var maximumMessage = new Random().Next();
            var query = TestHelper.GenerateString();
            var expectedResult = TestHelper.GenerateString();
            ArrangeQueryGeneratorGetLatestMessagesReceived(maximumMessage, query);
            _fakeTwitterAccessor.ArrangeExecuteJsonGETQuery(query, expectedResult);

            // Act
            var result = jsonController.GetLatestMessagesReceived(maximumMessage);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        private void ArrangeQueryGeneratorGetLatestMessagesReceived(int maximumMessages, string query)
        {
            _fakeMessageQueryGenerator
                .CallsTo(x => x.GetLatestMessagesReceivedQuery(maximumMessages))
                .Returns(query);
        }

        #endregion

        #region GetLatestMessagesSent

        [TestMethod]
        public void GetLatestMessagesSent_ReturnsTwitterAccessorResult()
        {
            // Arrange
            var jsonController = CreateMessageJsonController();
            var maximumMessage = new Random().Next();
            var query = TestHelper.GenerateString();
            var expectedResult = TestHelper.GenerateString();
            ArrangeQueryGeneratorGetLatestMessagesSent(maximumMessage, query);
            _fakeTwitterAccessor.ArrangeExecuteJsonGETQuery(query, expectedResult);

            // Act
            var result = jsonController.GetLatestMessagesSent(maximumMessage);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        private void ArrangeQueryGeneratorGetLatestMessagesSent(int maximumMessages, string query)
        {
            _fakeMessageQueryGenerator
                .CallsTo(x => x.GetLatestMessagesSentQuery(maximumMessages))
                .Returns(query);
        }

        #endregion

        #region Destroy Message

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DestroyMessage_WithNullMessage_ThrowArgumentException()
        {
            // Arrange
            var jsonController = CreateMessageJsonController();

            var query = TestHelper.GenerateString();
            var expectedResult = TestHelper.GenerateString();

            _fakeMessageQueryGenerator.CallsTo(x => x.GetDestroyMessageQuery(A.Fake<IMessageDTO>())).Returns(query);
            _fakeTwitterAccessor.ArrangeExecuteJsonPOSTQuery(query, expectedResult);

            // Act
            jsonController.DestroyMessage((IMessage)null);
        }

        [TestMethod]
        public void VerifyDestroyMessageWithTextAndMessage()
        {
            // Arrange
            var jsonController = CreateMessageJsonController();
            var message = A.Fake<IMessage>();
            var messageDTO = A.Fake<IMessageDTO>();
                message.CallsTo(x => x.MessageDTO).Returns(messageDTO);

            var query = TestHelper.GenerateString();
            var expectedResult = TestHelper.GenerateString();

            _fakeMessageQueryGenerator.CallsTo(x => x.GetDestroyMessageQuery(messageDTO)).Returns(query);
            _fakeTwitterAccessor.ArrangeExecuteJsonPOSTQuery(query, expectedResult);

            // Act
            var result = jsonController.DestroyMessage(message);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void DestroyMessage_WithMessageDTO_AlwaysReturnTwitterAccessor()
        {
            // Arrange
            var jsonController = CreateMessageJsonController();
            var messageDTO = A.Fake<IMessageDTO>();
            var query = TestHelper.GenerateString();
            var expectedResult = TestHelper.GenerateString();

            _fakeMessageQueryGenerator.CallsTo(x => x.GetDestroyMessageQuery(messageDTO)).Returns(query);
            _fakeTwitterAccessor.ArrangeExecuteJsonPOSTQuery(query, expectedResult);

            // Act
            var result = jsonController.DestroyMessage(messageDTO);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void DestroyMessage_WithMessageId_AlwaysReturnTwitterAccessor()
        {
            // Arrange
            var jsonController = CreateMessageJsonController();
            var messageId = TestHelper.GenerateRandomLong();
            var query = TestHelper.GenerateString();
            var expectedResult = TestHelper.GenerateString();

            _fakeMessageQueryGenerator.CallsTo(x => x.GetDestroyMessageQuery(messageId)).Returns(query);
            _fakeTwitterAccessor.ArrangeExecuteJsonPOSTQuery(query, expectedResult);

            // Act
            var result = jsonController.DestroyMessage(messageId);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        #endregion

        public MessageJsonController CreateMessageJsonController()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}