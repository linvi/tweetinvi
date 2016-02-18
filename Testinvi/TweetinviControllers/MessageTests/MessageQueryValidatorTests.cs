using System;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi.Controllers.Messages;
using Tweetinvi.Core;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.QueryValidators;
using Tweetinvi.Core.Parameters;

namespace Testinvi.TweetinviControllers.MessageTests
{
    [TestClass]
    public class MessageQueryValidatorTests
    {
        private FakeClassBuilder<MessageQueryValidator> _fakeBuilder;
        private Fake<IUserQueryValidator> _fakeUserQueryValidator;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<MessageQueryValidator>();
            _fakeUserQueryValidator = _fakeBuilder.GetFake<IUserQueryValidator>();
            _fakeUserQueryValidator.ArrangeIsUserIdValid();
        }

        #region Can Message Be Published

        [TestMethod]
        public void CanMessageBePublished_BasedOnMessageHasBeenPublishedOrDestroyed()
        {
            // Arrange - Act
            var result1 = CanMessageBePublished_BasedOnPublishedAndDestroyed(true, true);
            var result2 = CanMessageBePublished_BasedOnPublishedAndDestroyed(true, false);
            var result3 = CanMessageBePublished_BasedOnPublishedAndDestroyed(false, true);
            var result4 = CanMessageBePublished_BasedOnPublishedAndDestroyed(false, false);

            // Assert
            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsTrue(result4);
        }

        private bool CanMessageBePublished_BasedOnPublishedAndDestroyed(bool messageHasBeenPublished, bool messageHasBeenDestroyed)
        {
            // Arrange
            var queryValidator = CreateMessageQueryValidator();
            var messageDTO = CreateMessageDTO(messageHasBeenPublished, messageHasBeenDestroyed);

            var parameters = A.Fake<IPublishMessageParameters>();
            parameters.CallsTo(x => x.Message).Returns(messageDTO);

            ArrangeMessagePublishParameterText(parameters, true, true);
            ArrangeMessageDTORecipient(parameters, true, true, true);

            // Act
            return queryValidator.CanMessageBePublished(parameters);
        }

        [TestMethod]
        public void CanMessageDTOBePublished_BasedOnText()
        {
            // Arrange - Act
            var result1 = CanMessageDTOBePublished_BasedOnText(true, true);
            var result2 = CanMessageDTOBePublished_BasedOnText(true, false);
            var result3 = CanMessageDTOBePublished_BasedOnText(false, true);
            var result4 = CanMessageDTOBePublished_BasedOnText(false, false);

            // Assert
            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
            Assert.IsFalse(result4);
        }

        private bool CanMessageDTOBePublished_BasedOnText(bool doesTextExists, bool textContainsChars)
        {
            // Arrange
            var queryValidator = CreateMessageQueryValidator();

            var parameters = A.Fake<IPublishMessageParameters>();
            parameters.CallsTo(x => x.Message).Returns(null);

            ArrangeMessagePublishParameterText(parameters, doesTextExists, textContainsChars);

            // Act
            return queryValidator.CanMessageBePublished(parameters);
        }

        [TestMethod]
        public void CanMessageBePublished_BasedOnRecipient()
        {
            // Arrange - Act
            var result1 = CanMessageBePublished_BasedOnRecipient(true, true, true);
            var result2 = CanMessageBePublished_BasedOnRecipient(true, true, false);
            var result3 = CanMessageBePublished_BasedOnRecipient(true, false, true);
            var result4 = CanMessageBePublished_BasedOnRecipient(true, false, false);

            var result5 = CanMessageBePublished_BasedOnRecipient(false, true, true);
            var result6 = CanMessageBePublished_BasedOnRecipient(false, true, false);
            var result7 = CanMessageBePublished_BasedOnRecipient(false, false, true);
            var result8 = CanMessageBePublished_BasedOnRecipient(false, false, false);

            // Assert
            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsTrue(result3);
            Assert.IsTrue(result4);
            Assert.IsTrue(result5);
            Assert.IsTrue(result6);
            Assert.IsTrue(result7);
            Assert.IsFalse(result8);
        }

        private bool CanMessageBePublished_BasedOnRecipient( bool isRecipientValid, bool isRecipientIdValid, bool isRecipientScreenNameValid)
        {
            // Arrange
            var queryValidator = CreateMessageQueryValidator();
            var parameter = A.Fake<IPublishMessageParameters>();
            parameter.CallsTo(x => x.Text).Returns(TestHelper.GenerateString());

            ArrangeMessageDTORecipient(parameter, isRecipientValid, isRecipientIdValid, isRecipientScreenNameValid);

            // Act
            return queryValidator.CanMessageBePublished(parameter);
        }

        private void ArrangeMessagePublishParameterText(IPublishMessageParameters parameters, bool doesTextExists, bool textContainsChars)
        {
            string text = doesTextExists ? textContainsChars ? TestHelper.GenerateString() : string.Empty : null;

            parameters.CallsTo(x => x.Text).Returns(text);
        }

        private void ArrangeMessageDTORecipient(
            IPublishMessageParameters parameters,
            bool isRecipientValid,
            bool isRecipientIdValid,
            bool isRecipientScreenNameValid)
        {
            var recipient = A.Fake<IUserDTO>();
            var recipientId = TestHelper.GenerateRandomLong();
            var recipientScreenName = TestHelper.GenerateString();

            parameters.CallsTo(x => x.Recipient).Returns(recipient);
            parameters.CallsTo(x => x.RecipientId).Returns(recipientId);
            parameters.CallsTo(x => x.RecipientScreenName).Returns(recipientScreenName);

            _fakeUserQueryValidator.CallsTo(x => x.CanUserBeIdentified(recipient)).Returns(isRecipientValid);
            _fakeUserQueryValidator.CallsTo(x => x.IsUserIdValid(recipientId)).Returns(isRecipientIdValid);
            _fakeUserQueryValidator.CallsTo(x => x.IsScreenNameValid(recipientScreenName)).Returns(isRecipientScreenNameValid);
        }

        #endregion

        #region Can Message Be Destroyed

        [TestMethod]
        public void CanMessageDTOBeDestroyed_BasedOnMessageHasBeenPublishedOrDestroyed()
        {
            // Arrange - Act
            var result1 = CanMessageDTOBeDestroyed_BasedOnPublishedAndDestroyed(true, true, true);
            var result2 = CanMessageDTOBeDestroyed_BasedOnPublishedAndDestroyed(true, true, false);
            var result3 = CanMessageDTOBeDestroyed_BasedOnPublishedAndDestroyed(true, false, true);
            var result4 = CanMessageDTOBeDestroyed_BasedOnPublishedAndDestroyed(true, false, false);

            var result5 = CanMessageDTOBeDestroyed_BasedOnPublishedAndDestroyed(false, true, true);
            var result6 = CanMessageDTOBeDestroyed_BasedOnPublishedAndDestroyed(false, true, false);
            var result7 = CanMessageDTOBeDestroyed_BasedOnPublishedAndDestroyed(false, false, true);
            var result8 = CanMessageDTOBeDestroyed_BasedOnPublishedAndDestroyed(false, false, false);

            // Assert
            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsTrue(result3);
            Assert.IsFalse(result4);

            Assert.IsFalse(result5);
            Assert.IsFalse(result6);
            Assert.IsFalse(result7);
            Assert.IsFalse(result8);
        }

        private bool CanMessageDTOBeDestroyed_BasedOnPublishedAndDestroyed(
            bool messageHasBeenPublished,
            bool messageHasBeenDestroyed,
            bool isMessageIdSetup)
        {
            // Arrange
            var queryValidator = CreateMessageQueryValidator();
            var messageDTO = CreateMessageDTO(messageHasBeenPublished, messageHasBeenDestroyed);
            messageDTO.Id = isMessageIdSetup ? new Random().Next() : TweetinviSettings.DEFAULT_ID;

            // Act
            return queryValidator.CanMessageDTOBeDestroyed(messageDTO);
        }

        [TestMethod]
        public void CanMessageDTOBeDestroyed_MessageDTOIsNull_ReturnsFalse()
        {
            // Arrange
            var queryValidator = CreateMessageQueryValidator();

            // Act
            var result = queryValidator.CanMessageDTOBeDestroyed(null);

            // Assert
            Assert.IsFalse(result);
        }

        #endregion

        #region Is Text Message Valid

        [TestMethod]
        public void IsTextMessageValid_IsNull_ReturnsFalse()
        {
            // Arrange
            var queryValidator = CreateMessageQueryValidator();

            // Act
            var result = queryValidator.IsMessageTextValid(null);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsTextMessageValid_IsEmpty_ReturnsFalse()
        {
            // Arrange
            var queryValidator = CreateMessageQueryValidator();

            // Act
            var result = queryValidator.IsMessageTextValid(String.Empty);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsTextMessageValid_IsRandom_ReturnsTrue()
        {
            string message = TestHelper.GenerateString();

            // Arrange
            var queryValidator = CreateMessageQueryValidator();

            // Act
            var result = queryValidator.IsMessageTextValid(message);

            // Assert
            Assert.IsTrue(result);
        }

        #endregion

        #region Is User Id Valid

        [TestMethod]
        public void IsMessageIdValid_IsDefault_ReturnsFalse()
        {
            // Arrange
            var queryValidator = CreateMessageQueryValidator();

            // Act
            var result = queryValidator.IsMessageIdValid(TestHelper.DefaultId());

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsMessageIdValid_IsRandom_ReturnsTrue()
        {
            var id = TestHelper.GenerateRandomLong();

            // Arrange
            var queryValidator = CreateMessageQueryValidator();

            // Act
            var result = queryValidator.IsMessageIdValid(id);

            // Assert
            Assert.IsTrue(result);
        }

        #endregion

        private IMessageDTO CreateMessageDTO(
           bool hasBeenPublished,
           bool hasBeenDestroyed)
        {
            var messageDTO = A.Fake<IMessageDTO>();

            messageDTO.CallsTo(x => x.IsMessagePublished).Returns(hasBeenPublished);
            messageDTO.CallsTo(x => x.IsMessageDestroyed).Returns(hasBeenDestroyed);

            return messageDTO;
        }

        public MessageQueryValidator CreateMessageQueryValidator()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}