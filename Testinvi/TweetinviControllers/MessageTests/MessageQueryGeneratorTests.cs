using System;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi.Controllers.Messages;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.QueryGenerators;
using Tweetinvi.Core.Interfaces.QueryValidators;

namespace Testinvi.TweetinviControllers.MessageTests
{
    [TestClass]
    public class MessageQueryGeneratorTests
    {
        private FakeClassBuilder<MessageQueryGenerator> _fakeBuilder;
        private Fake<IMessageQueryValidator> _fakeMessageQueryValidator;
        private Fake<IUserQueryParameterGenerator> _fakeUserQueryParameterGenerator;
        private Fake<IUserQueryValidator> _fakeUserQueryValidator;
        private Fake<ITwitterStringFormatter> _fakeTwitterStringFormatter;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<MessageQueryGenerator>();
            _fakeMessageQueryValidator = _fakeBuilder.GetFake<IMessageQueryValidator>();
            _fakeUserQueryParameterGenerator = _fakeBuilder.GetFake<IUserQueryParameterGenerator>();
            _fakeUserQueryValidator = _fakeBuilder.GetFake<IUserQueryValidator>();
            _fakeTwitterStringFormatter = _fakeBuilder.GetFake<ITwitterStringFormatter>();

            _fakeUserQueryParameterGenerator.ArrangeGenerateIdParameter();
            _fakeUserQueryParameterGenerator.ArrangeGenerateScreenNameParameter();
        }

        #region PublishMessage Query

        [TestMethod]
        public void GetPublishMessageQuery_WithTextAndMessageDTO_ExpectedQuery()
        {
            VerifyGetPublishMessageQuery_WithTextAndMessageDTO(true, true, true, true, true);
            VerifyGetPublishMessageQuery_WithTextAndMessageDTO(true, true, true, false, true);
            VerifyGetPublishMessageQuery_WithTextAndMessageDTO(true, true, false, true, true);
            VerifyGetPublishMessageQuery_WithTextAndMessageDTO(true, true, false, false, true);

            VerifyGetPublishMessageQuery_WithTextAndMessageDTO(true, false, true, true, true);
            VerifyGetPublishMessageQuery_WithTextAndMessageDTO(true, false, true, false, true);
            VerifyGetPublishMessageQuery_WithTextAndMessageDTO(true, false, false, true, true);
            VerifyGetPublishMessageQuery_WithTextAndMessageDTO(true, false, false, false, false);

            VerifyGetPublishMessageQuery_WithTextAndMessageDTO(false, true, true, true, false);
            VerifyGetPublishMessageQuery_WithTextAndMessageDTO(false, true, true, false, false);
            VerifyGetPublishMessageQuery_WithTextAndMessageDTO(false, true, false, true, false);
            VerifyGetPublishMessageQuery_WithTextAndMessageDTO(false, true, false, false, false);

            VerifyGetPublishMessageQuery_WithTextAndMessageDTO(false, false, true, true, false);
            VerifyGetPublishMessageQuery_WithTextAndMessageDTO(false, false, true, false, false);
            VerifyGetPublishMessageQuery_WithTextAndMessageDTO(false, false, false, true, false);
            VerifyGetPublishMessageQuery_WithTextAndMessageDTO(false, false, false, false, false);
        }

        public void VerifyGetPublishMessageQuery_WithTextAndMessageDTO(
            bool canMessageBePublished,
            bool isRecipientValid,
            bool isRecipientIdValid,
            bool isRecipientScreenNameValid,
            bool expectedValue)
        {
            var text = TestHelper.GenerateString();
            var twitterText = TestHelper.GenerateString();

            _fakeTwitterStringFormatter.CallsTo(x => x.TwitterEncode(It.IsAny<string>())).Returns(twitterText);

            _fakeMessageQueryValidator.CallsTo(x => x.IsMessageTextValid(text)).Returns(canMessageBePublished);
            //_fakeMessageQueryValidator.CallsTo(x => x.CanMessageDTOBePublished(messageDTO)).Returns(canMessageBePublished);

            //ArrangeMessageDTORecipient(messageDTO, isRecipientValid, isRecipientIdValid, isRecipientScreenNameValid);

            // Arrange
            var queryGenerator = CreateMessageQueryGenerator();
            var expectedIdentifierParameter = Guid.NewGuid().ToString();
            var expectedResult = string.Format(Resources.Message_NewMessage, twitterText, expectedIdentifierParameter);

            if (isRecipientValid)
            {
                _fakeUserQueryParameterGenerator.ArrangeGenerateIdOrScreenNameParameter(expectedIdentifierParameter);
            }
            else if (isRecipientIdValid)
            {
                _fakeUserQueryParameterGenerator.ArrangeGenerateIdParameter(expectedIdentifierParameter);
            }
            else
            {
                _fakeUserQueryParameterGenerator.ArrangeGenerateScreenNameParameter(expectedIdentifierParameter);
            }

            // Act
            //var result = queryGenerator.GetPublishMessageQuery(messageDTO);

            // Assert
            //Assert.AreEqual(result, expectedValue ? expectedResult : null);
        }

        [TestMethod]
        public void GetPublishMessageQuery_WithTextAndUserDTO_ExpectedQuery()
        {
            VerifyGetPublishMessageQuery_WithTextAndUserDTO(true, true, true);
            VerifyGetPublishMessageQuery_WithTextAndUserDTO(true, false, false);
            VerifyGetPublishMessageQuery_WithTextAndUserDTO(false, true, false);
            VerifyGetPublishMessageQuery_WithTextAndUserDTO(false, false, false);
        }

        private void VerifyGetPublishMessageQuery_WithTextAndUserDTO(
            bool isTextValid,
            bool isUserValid,
            bool expectValue)
        {
            var text = TestHelper.GenerateString();
            var userDTO = A.Fake<IUserDTO>();
            var twitterText = TestHelper.GenerateString();

            _fakeTwitterStringFormatter.CallsTo(x => x.TwitterEncode(It.IsAny<string>())).Returns(twitterText);
            _fakeMessageQueryValidator.CallsTo(x => x.IsMessageTextValid(text)).Returns(isTextValid);
            _fakeUserQueryValidator.CallsTo(x => x.CanUserBeIdentified(userDTO)).Returns(isUserValid);

            var expectedIdentifierParameter = Guid.NewGuid().ToString();
            var expectedResult = string.Format(Resources.Message_NewMessage, twitterText, expectedIdentifierParameter);

            // Arrange
            var queryGenerator = CreateMessageQueryGenerator();
            _fakeUserQueryParameterGenerator.ArrangeGenerateIdOrScreenNameParameter(expectedIdentifierParameter);

            // Act
            //var result = queryGenerator.GetPublishMessageQuery(text, userDTO);

            // Assert
            //Assert.AreEqual(result, expectValue ? expectedResult : null);
        }

        [TestMethod]
        public void GetPublishMessageQuery_WithTextAndScreenName_ExpectedQuery()
        {
            VerifyGetPublishMessageQuery_WithTextAndScreenName(true, true, true);
            VerifyGetPublishMessageQuery_WithTextAndScreenName(true, false, false);
            VerifyGetPublishMessageQuery_WithTextAndScreenName(false, true, false);
            VerifyGetPublishMessageQuery_WithTextAndScreenName(false, false, false);
        }

        public void VerifyGetPublishMessageQuery_WithTextAndScreenName(
            bool isTextValid,
            bool isUserValid,
            bool expectValue)
        {
            var text = TestHelper.GenerateString();
            var screenName = TestHelper.GenerateString();
            var twitterText = TestHelper.GenerateString();

            _fakeTwitterStringFormatter.CallsTo(x => x.TwitterEncode(It.IsAny<string>())).Returns(twitterText);
            _fakeMessageQueryValidator.CallsTo(x => x.IsMessageTextValid(text)).Returns(isTextValid);
            _fakeUserQueryValidator.CallsTo(x => x.IsScreenNameValid(screenName)).Returns(isUserValid);

            var expectedIdentifierParameter = UserQueryGeneratorHelper.GenerateParameterExpectedResult(screenName);
            var expectedResult = string.Format(Resources.Message_NewMessage, twitterText, expectedIdentifierParameter);

            // Arrange
            var queryGenerator = CreateMessageQueryGenerator();

            // Act
            //var result = queryGenerator.GetPublishMessageQuery(text, screenName);

            // Assert
            //Assert.AreEqual(result, expectValue ? expectedResult : null);
        }

        [TestMethod]
        public void GetPublishMessageQuery_WithTextAndUserId_ExpectedQuery()
        {
            VerifyGetPublishMessageQuery_WithParameter(true, true, true);
            VerifyGetPublishMessageQuery_WithParameter(true, false, false);
            VerifyGetPublishMessageQuery_WithParameter(false, true, false);
            VerifyGetPublishMessageQuery_WithParameter(false, false, false);
        }

        public void VerifyGetPublishMessageQuery_WithParameter(
            bool isTextValid,
            bool isUserIdValid,
            bool expectValue)
        {
            var text = TestHelper.GenerateString();
            var userId = TestHelper.GenerateRandomLong();
            var twitterText = TestHelper.GenerateString();

            _fakeTwitterStringFormatter.CallsTo(x => x.TwitterEncode(It.IsAny<string>())).Returns(twitterText);
            _fakeMessageQueryValidator.CallsTo(x => x.IsMessageTextValid(text)).Returns(isTextValid);
            _fakeUserQueryValidator.CallsTo(x => x.IsUserIdValid(userId)).Returns(isUserIdValid);

            var expectedIdentifierParameter = UserQueryGeneratorHelper.GenerateParameterExpectedResult(userId);
            var expectedResult = string.Format(Resources.Message_NewMessage, twitterText, expectedIdentifierParameter);

            // Arrange
            var queryGenerator = CreateMessageQueryGenerator();

            // Act
            //var result = queryGenerator.GetPublishMessageQuery(text, userId);

            // Assert
            //Assert.AreEqual(result, expectValue ? expectedResult : null);
        }

        #endregion

        #region Destroy Query

        [TestMethod]
        public void DestroyMessage_ExpectedQuery()
        {
            // Verify
            VerifyDestroyMessage_ExpectedQuery(true, true, true);
            VerifyDestroyMessage_ExpectedQuery(true, false, false);
            VerifyDestroyMessage_ExpectedQuery(false, true, false);
            VerifyDestroyMessage_ExpectedQuery(false, false, false);
        }

        public void VerifyDestroyMessage_ExpectedQuery(bool canMessageBeDestroyed, bool isMessageIdValid, bool resultExists)
        {
            var messageId = TestHelper.GenerateRandomLong();

            // Arrange
            var queryGenerator = CreateMessageQueryGenerator();
            var messageDTO = A.Fake<IMessageDTO>();
            messageDTO.CallsTo(x => x.Id).Returns(messageId);

            _fakeMessageQueryValidator.CallsTo(x => x.CanMessageDTOBeDestroyed(messageDTO)).Returns(canMessageBeDestroyed);
            _fakeMessageQueryValidator.CallsTo(x => x.IsMessageIdValid(messageId)).Returns(isMessageIdValid);

            // Act
            var result = queryGenerator.GetDestroyMessageQuery(messageDTO);

            // Assert
            string expectedMessage = string.Format(Resources.Message_DestroyMessage, messageId);
            Assert.AreEqual(result, resultExists ? expectedMessage : null);
        }

        #endregion

        private void ArrangeMessageDTORecipient(
            IMessageDTO messageDTO,
            bool isRecipientValid,
            bool isRecipientIdValid,
            bool isRecipientScreenNameValid)
        {
            var recipient = A.Fake<IUserDTO>();
            var recipientId = TestHelper.GenerateRandomLong();
            var recipientScreenName = TestHelper.GenerateString();

            messageDTO.CallsTo(x => x.Recipient).Returns(recipient);
            messageDTO.CallsTo(x => x.RecipientId).Returns(recipientId);
            messageDTO.CallsTo(x => x.RecipientScreenName).Returns(recipientScreenName);

            _fakeUserQueryValidator.CallsTo(x => x.CanUserBeIdentified(recipient)).Returns(isRecipientValid);
            _fakeUserQueryValidator.CallsTo(x => x.IsUserIdValid(recipientId)).Returns(isRecipientIdValid);
            _fakeUserQueryValidator.CallsTo(x => x.IsScreenNameValid(recipientScreenName)).Returns(isRecipientScreenNameValid);
        }

        public MessageQueryGenerator CreateMessageQueryGenerator()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}