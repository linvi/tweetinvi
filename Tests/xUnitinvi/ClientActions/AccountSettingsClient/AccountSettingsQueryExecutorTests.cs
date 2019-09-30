using System.Threading.Tasks;
using FakeItEasy;
using Tweetinvi;
using Tweetinvi.Controllers.AccountSettings;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;
using Xunit;
using xUnitinvi.TestHelpers;

namespace xUnitinvi.ClientActions.AccountSettingsClient
{
    public class AccountSettingsQueryExecutorTests
    {
        public AccountSettingsQueryExecutorTests()
        {
            _fakeBuilder = new FakeClassBuilder<AccountSettingsQueryExecutor>();
            _fakeAccountSettingsQueryGenerator = _fakeBuilder.GetFake<IAccountSettingsQueryGenerator>().FakedObject;
            _fakeTwitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>().FakedObject;
        }

        private readonly FakeClassBuilder<AccountSettingsQueryExecutor> _fakeBuilder;
        private readonly IAccountSettingsQueryGenerator _fakeAccountSettingsQueryGenerator;
        private readonly ITwitterAccessor _fakeTwitterAccessor;

        private AccountSettingsQueryExecutor CreateAccountSettingsQueryExecutor()
        {
            return _fakeBuilder.GenerateClass();
        }

        [Fact]
        public async Task UpdateProfileImage_ReturnsAccountUserDTO()
        {
            // Arrange
            var queryExecutor = CreateAccountSettingsQueryExecutor();
            var parameters = new UpdateProfileImageParameters(null);

            var query = TestHelper.GenerateString();
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterResult<IUserDTO>>();

            A.CallTo(() => _fakeAccountSettingsQueryGenerator.GetUpdateProfileImageQuery(parameters)).Returns(query);
            A.CallTo(() => _fakeTwitterAccessor.ExecuteRequest<IUserDTO>(A<ITwitterRequest>
                    .That.Matches(x => x.Query is IMultipartTwitterQuery && x.Query.Url == query)))
                .Returns(expectedResult);

            // Act
            var result = await queryExecutor.UpdateProfileImage(parameters, request);

            // Assert
            Assert.Equal(result, expectedResult);
        }
    }
}