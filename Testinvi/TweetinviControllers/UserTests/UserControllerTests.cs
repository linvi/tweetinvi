using System;
using System.IO;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.Controllers.User;
using Tweetinvi.Core.DTO.Cursor;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;

namespace Testinvi.TweetinviControllers.UserTests
{
    [TestClass]
    public class UserControllerTests
    {
        private FakeClassBuilder<UserController> _fakeBuilder;
        private Fake<IUserQueryExecutor> _fakeUserQueryExecutor;
        private Fake<ITweetFactory> _fakeTweetFactory;
        private Fake<IUserFactory> _fakeUserFactory;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<UserController>();
            _fakeUserQueryExecutor = _fakeBuilder.GetFake<IUserQueryExecutor>();
            _fakeTweetFactory = _fakeBuilder.GetFake<ITweetFactory>();
            _fakeUserFactory = _fakeBuilder.GetFake<IUserFactory>();
        }

        #region Stream Profile Image

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GenerateProfileImageStream_WithNullUser_ThrowsArgumentException()
        {
            // Arrange
            var controller = CreateUserController();

            // Act
            controller.GetProfileImageStream((IUser)null, ImageSize.bigger);
        }

        [TestMethod]
        public void GenerateProfileImageStream_WithUser_ReturnQueryExecutorStream()
        {
            // Arrange
            var controller = CreateUserController();
            var userDTO = A.Fake<IUserDTO>();
            var user = TestHelper.GenerateUser(userDTO);
            var stream = A.Fake<Stream>();

            _fakeUserQueryExecutor.CallsTo(x => x.GetProfileImageStream(userDTO, ImageSize.bigger)).Returns(stream);

            // Act
            var result = controller.GetProfileImageStream(user, ImageSize.bigger);

            // Assert
            Assert.AreEqual(result, stream);
        }

        [TestMethod]
        public void GetProfileImageStream_WithUserDTO_ReturnQueryExecutorStream()
        {
            // Arrange
            var controller = CreateUserController();
            var userDTO = A.Fake<IUserDTO>();
            var stream = A.Fake<Stream>();

            _fakeUserQueryExecutor.CallsTo(x => x.GetProfileImageStream(userDTO, ImageSize.bigger)).Returns(stream);

            // Act
            var result = controller.GetProfileImageStream(userDTO, ImageSize.bigger);

            // Assert
            Assert.AreEqual(result, stream);
        }
        #endregion

        private UserController CreateUserController()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}