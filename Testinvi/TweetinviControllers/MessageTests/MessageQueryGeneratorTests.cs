using System;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
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
        private Fake<IUserQueryParameterGenerator> _fakeUserQueryParameterGenerator;
        private Fake<ITwitterStringFormatter> _fakeTwitterStringFormatter;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<MessageQueryGenerator>();
            _fakeMessageQueryValidator = _fakeBuilder.GetFake<IMessageQueryValidator>();
            _fakeUserQueryParameterGenerator = _fakeBuilder.GetFake<IUserQueryParameterGenerator>();
            _fakeTwitterStringFormatter = _fakeBuilder.GetFake<ITwitterStringFormatter>();

            _fakeUserQueryParameterGenerator.ArrangeGenerateIdParameter();
            _fakeUserQueryParameterGenerator.ArrangeGenerateScreenNameParameter();
        }

        #region PublishMessage Query

        [TestMethod]
        public void PublishMessage_ExpectedQuery()
        {
            var text = TestHelper.GenerateString();
            var formattedText = TestHelper.GenerateString();
            var user = A.Fake<IUserIdentifier>();

            var parameter = A.Fake<IPublishMessageParameters>();

            parameter.CallsTo(x => x.Text).Returns(text);
            parameter.CallsTo(x => x.Recipient).Returns(user);

            _fakeTwitterStringFormatter.CallsTo(x => x.TwitterEncode(text)).Returns(formattedText);

            // Arrange
            var queryGenerator = CreateMessageQueryGenerator();
            var expectedIdentifierParameter = Guid.NewGuid().ToString();
            var expectedResult = string.Format(Resources.Message_NewMessage, formattedText, expectedIdentifierParameter);

            _fakeUserQueryParameterGenerator.ArrangeGenerateIdOrScreenNameParameter(expectedIdentifierParameter);

            // Act
            var result = queryGenerator.GetPublishMessageQuery(parameter);

            // Assert
            Assert.AreEqual(result, expectedResult);

            _fakeMessageQueryValidator.CallsTo(x => x.ThrowIfMessageCannotBePublished(parameter)).MustHaveHappened();
        }

        #endregion

        #region Destroy Query

        [TestMethod]
        public void DestroyMessage_ExpectedQuery()
        {
            // Arrange
            var messageId = TestHelper.GenerateRandomLong();
            var queryGenerator = CreateMessageQueryGenerator();
            var message = A.Fake<IMessageDTO>();
            message.CallsTo(x => x.Id).Returns(messageId);

            // Act
            var result = queryGenerator.GetDestroyMessageQuery(message);

            // Assert
            string expectedMessage = string.Format(Resources.Message_DestroyMessage, messageId);
            Assert.AreEqual(result,expectedMessage);

            _fakeMessageQueryValidator.CallsTo(x => x.ThrowIfMessageCannotBeDestroyed(message)).MustHaveHappened();
        }

        #endregion

        public MessageQueryGenerator CreateMessageQueryGenerator()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}