using System;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi.Controllers.Messages;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Testinvi.TweetinviControllers.MessageTests
{
    [TestClass]
    public class MessageQueryGeneratorTests
    {
        private FakeClassBuilder<MessageQueryGenerator> _fakeBuilder;
        private Fake<IMessageQueryValidator> _fakeMessageQueryValidator;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<MessageQueryGenerator>();
            _fakeMessageQueryValidator = _fakeBuilder.GetFake<IMessageQueryValidator>();
        }

        #region PublishMessage Query

        [TestMethod]
        public void PublishMessage_ExpectedQuery()
        {
            var text = TestHelper.GenerateString();
            var userId = TestHelper.GenerateRandomLong();

            var parameters = A.Fake<IPublishMessageParameters>();

            A.CallTo(() => parameters.Text).Returns(text);
            A.CallTo(() => parameters.RecipientId).Returns(userId);

            // Arrange
            var queryGenerator = CreateMessageQueryGenerator();

            // Act
            var result = queryGenerator.GetPublishMessageQuery(parameters);

            // Assert - query string should be just the URL. Parameters are sent in the body of the request
            Assert.AreEqual(result, Resources.Message_NewMessage);

            _fakeMessageQueryValidator.CallsTo(x => x.ThrowIfMessageCannotBePublished(parameters)).MustHaveHappened();
        }

        #endregion

        #region Destroy Query

        [TestMethod]
        public void DestroyMessage_ExpectedQuery()
        {
            // Arrange
            var messageId = TestHelper.GenerateRandomLong();
            var queryGenerator = CreateMessageQueryGenerator();
            var eventDTO = A.Fake<IEventDTO>();
            A.CallTo(() => eventDTO.Id).Returns(messageId);

            // Act
            var result = queryGenerator.GetDestroyMessageQuery(eventDTO);

            // Assert
            string expectedMessage = string.Format(Resources.Message_DestroyMessage, messageId);
            Assert.AreEqual(result,expectedMessage);

            _fakeMessageQueryValidator.CallsTo(x => x.ThrowIfMessageCannotBeDestroyed(eventDTO)).MustHaveHappened();
        }

        #endregion

        public MessageQueryGenerator CreateMessageQueryGenerator()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}