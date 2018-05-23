using System;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi.Controllers.Messages;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

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

        #region GetLatestMessages

        [TestMethod]
        public void GetLatestMessages_ReturnsTwitterAccessorResult()
        {
            // Arrange
            var jsonController = CreateMessageJsonController();
            var count = new Random().Next();
            var query = TestHelper.GenerateString();
            var expectedResult = TestHelper.GenerateString();
            ArrangeQueryGeneratorGetLatestMessages(count, query);
            _fakeTwitterAccessor.ArrangeExecuteJsonGETQuery(query, expectedResult);

            // Act
            var result = jsonController.GetLatestMessages(count);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        private void ArrangeQueryGeneratorGetLatestMessages(int count, string query)
        {
            _fakeMessageQueryGenerator
                .CallsTo(x => x.GetLatestMessagesQuery(A<IGetMessagesParameters>.That.Matches(p => p.Count == count)))
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

            _fakeMessageQueryGenerator.CallsTo(x => x.GetDestroyMessageQuery(A.Fake<IEventDTO>())).Returns(query);
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
            var eventDTO = A.Fake<IEventDTO>();
                message.CallsTo(x => x.EventDTO).Returns(eventDTO);

            var query = TestHelper.GenerateString();

            _fakeMessageQueryGenerator.CallsTo(x => x.GetDestroyMessageQuery(eventDTO)).Returns(query);
            _fakeTwitterAccessor.ArrangeTryExecuteDELETEQuery(query, true);

            // Act
            var result = jsonController.DestroyMessage(message);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void DestroyMessage_WithMessageDTO_AlwaysReturnTwitterAccessor()
        {
            // Arrange
            var jsonController = CreateMessageJsonController();
            var eventDTO = A.Fake<IEventDTO>();
            var query = TestHelper.GenerateString();

            _fakeMessageQueryGenerator.CallsTo(x => x.GetDestroyMessageQuery(eventDTO)).Returns(query);
            _fakeTwitterAccessor.ArrangeTryExecuteDELETEQuery(query, true);

            // Act
            var result = jsonController.DestroyMessage(eventDTO);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void DestroyMessage_WithMessageId_AlwaysReturnTwitterAccessor()
        {
            // Arrange
            var jsonController = CreateMessageJsonController();
            var messageId = TestHelper.GenerateRandomLong();
            var query = TestHelper.GenerateString();

            _fakeMessageQueryGenerator.CallsTo(x => x.GetDestroyMessageQuery(messageId)).Returns(query);
            _fakeTwitterAccessor.ArrangeTryExecuteDELETEQuery(query, true);

            // Act
            var result = jsonController.DestroyMessage(messageId);

            // Assert
            Assert.IsTrue(result);
        }

        #endregion

        public MessageJsonController CreateMessageJsonController()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}