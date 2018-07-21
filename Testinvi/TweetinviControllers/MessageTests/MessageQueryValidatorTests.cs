using System;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi.Controllers.Messages;
using Tweetinvi.Controllers.User;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.QueryValidators;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

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
        public void CanMessageBePublished_BasedOnText()
        {
            // Arrange - Act - Assert
            CanMessageBePublished_BasedOnText(true, true, null);
            CanMessageBePublished_BasedOnText(true, false, typeof(ArgumentException));
            CanMessageBePublished_BasedOnText(false, true, typeof(ArgumentException));
            CanMessageBePublished_BasedOnText(false, false, typeof(ArgumentException));
        }

        private void CanMessageBePublished_BasedOnText(bool doesTextExists, bool textContainsChars, Type exceptionType)
        {
            // Arrange
            var queryValidator = CreateMessageQueryValidator();

            var parameters = A.Fake<IPublishMessageParameters>();
            parameters.CallsTo(x => x.RecipientId).Returns(TestHelper.GenerateRandomLong());

            ArrangeMessagePublishParameterText(parameters, doesTextExists, textContainsChars);

            // Act
            try
            {
                queryValidator.ThrowIfMessageCannotBePublished(parameters);

            }
            catch (Exception e)
            {
                if (e.GetType() == exceptionType)
                {
                    return;
                }

                throw new Exception("Exception was not of the correct type.");
            }

            if (exceptionType != null)
            {
                throw new Exception("Exception was expected.");
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CanMessageBePublished_BasedOnRecipientNotSpecified()
        {
            // Arrange
            var queryValidator = CreateMessageQueryValidator();
            var parameter = A.Fake<IPublishMessageParameters>();
            parameter.CallsTo(x => x.Text).Returns(TestHelper.GenerateString());

            // Act
            queryValidator.ThrowIfMessageCannotBePublished(parameter);
        }

        [TestMethod]
        public void CanMessageBePublished_BasedOnRecipientSpecified()
        {
            // Arrange
            var queryValidator = CreateMessageQueryValidator();
            var parameter = A.Fake<IPublishMessageParameters>();
            parameter.CallsTo(x => x.Text).Returns(TestHelper.GenerateString());
            parameter.CallsTo(x => x.RecipientId).Returns(TestHelper.GenerateRandomLong());

            // Act
            queryValidator.ThrowIfMessageCannotBePublished(parameter);

            // No exception thrown
        }

        private void ArrangeMessagePublishParameterText(IPublishMessageParameters parameters, bool doesTextExists, bool textContainsChars)
        {
            string text = doesTextExists ? textContainsChars ? TestHelper.GenerateString() : string.Empty : null;

            parameters.CallsTo(x => x.Text).Returns(text);
        }

        #endregion

        #region Can Message Be Destroyed

        [TestMethod]
        public void CanMessageDTOBeDestroyed_BasedOnMessageHasBeenDestroyed()
        {
            // Arrange - Act
            var result1 = CanMessageDTOBeDestroyed_BasedOnDestroyed(true);
            var result2 = CanMessageDTOBeDestroyed_BasedOnDestroyed(false);

            // Assert
            Assert.IsFalse(result1);
            Assert.IsTrue(result2);
        }

        private bool CanMessageDTOBeDestroyed_BasedOnDestroyed(bool messageHasBeenDestroyed)
        {
            // Arrange
            var queryValidator = CreateMessageQueryValidator();
            var eventDTO = CreateEventDTOForMessageCreate(messageHasBeenDestroyed);

            // Act
            try
            {
                queryValidator.ThrowIfMessageCannotBeDestroyed(eventDTO);
                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
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
            var result = queryValidator.IsMessageTextValid(string.Empty);

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
        [ExpectedException(typeof(ArgumentException))]
        public void IsMessageIdValid_IsDefault_ReturnsFalse()
        {
            // Arrange
            var queryValidator = CreateMessageQueryValidator();

            // Act
            queryValidator.ThrowIfMessageCannotBeDestroyed(TestHelper.DefaultId());
        }

        [TestMethod]
        public void IsMessageIdValid_IsRandom_ReturnsTrue()
        {
            var id = TestHelper.GenerateRandomLong();

            // Arrange
            var queryValidator = CreateMessageQueryValidator();

            // Act
            queryValidator.ThrowIfMessageCannotBeDestroyed(id);
        }

        #endregion

        private IEventDTO CreateEventDTOForMessageCreate(bool isDestroyed)
        {
            var messageDTO = A.Fake<IEventDTO>();
            messageDTO.Type = EventType.MessageCreate;
            messageDTO.MessageCreate = A.Fake<IMessageCreateDTO>();

            messageDTO.MessageCreate.CallsTo(x => x.IsDestroyed).Returns(isDestroyed);

            return messageDTO;
        }

        public MessageQueryValidator CreateMessageQueryValidator()
        {
            var userQueryValidator = new UserQueryValidator();
            return _fakeBuilder.GenerateClass(new ConstructorNamedParameter("userQueryValidator", userQueryValidator));
        }
    }
}