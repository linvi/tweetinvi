using System;
using FakeItEasy;
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
        private IMessageQueryGenerator _messageQueryGenerator;
        private ITwitterAccessor _twitterAccessor;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<MessageJsonController>();
            _messageQueryGenerator = _fakeBuilder.GetFake<IMessageQueryGenerator>().FakedObject;
            _twitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>().FakedObject;
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
            _twitterAccessor.ArrangeExecuteJsonGETQuery(query, expectedResult);

            // Act
            var result = jsonController.GetLatestMessages(count);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        private void ArrangeQueryGeneratorGetLatestMessages(int count, string query)
        {
            A.CallTo(() =>
                    _messageQueryGenerator.GetLatestMessagesQuery(
                        A<IGetMessagesParameters>.That.Matches(p => p.Count == count)))
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

            A.CallTo(() => _messageQueryGenerator.GetDestroyMessageQuery(A.Fake<IEventDTO>())).Returns(query);
            _twitterAccessor.ArrangeExecuteJsonPOSTQuery(query, expectedResult);

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
            A.CallTo(() => message.EventDTO).Returns(eventDTO);

            var query = TestHelper.GenerateString();

            A.CallTo(() => _messageQueryGenerator.GetDestroyMessageQuery(eventDTO)).Returns(query);
            _twitterAccessor.ArrangeTryExecuteDELETEQuery(query, true);

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

            A.CallTo(() => _messageQueryGenerator.GetDestroyMessageQuery(eventDTO)).Returns(query);
            _twitterAccessor.ArrangeTryExecuteDELETEQuery(query, true);

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

            A.CallTo(() => _messageQueryGenerator.GetDestroyMessageQuery(messageId)).Returns(query);
            _twitterAccessor.ArrangeTryExecuteDELETEQuery(query, true);

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