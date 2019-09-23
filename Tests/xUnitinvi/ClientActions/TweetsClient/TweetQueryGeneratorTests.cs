using System;
using FakeItEasy;
using Tweetinvi;
using Tweetinvi.Controllers.Shared;
using Tweetinvi.Controllers.Tweet;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.QueryValidators;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using Xunit;
using xUnitinvi.TestHelpers;

namespace xUnitinvi.ClientActions.TweetsClient
{
    public class TweetQueryGeneratorTests
    {
        public TweetQueryGeneratorTests()
        {
            _fakeBuilder = new FakeClassBuilder<TweetQueryGenerator>();
            _fakeUserQueryValidator = _fakeBuilder.GetFake<IUserQueryValidator>();
        }

        private readonly FakeClassBuilder<TweetQueryGenerator> _fakeBuilder;
        private readonly Fake<IUserQueryValidator> _fakeUserQueryValidator;

        private TweetQueryGenerator CreateUserQueryGenerator()
        {
            return _fakeBuilder.GenerateClass(
                new ConstructorNamedParameter("queryParameterGenerator", TweetinviContainer.Resolve<IQueryParameterGenerator>()),
                new ConstructorNamedParameter("userQueryParameterGenerator", TweetinviContainer.Resolve<IUserQueryParameterGenerator>()));
        }

        [Fact]
        public void GetFavoriteTweetsQuery_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateUserQueryGenerator();
            var user = new UserIdentifier(42);

            var parameters = new GetFavoriteTweetsParameters(user)
            {
                IncludeEntities = true,
                MaxId = 42,
                SinceId = 43,
                PageSize = 12,
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetFavoriteTweetsQuery(parameters, TweetMode.Extended);

            // Assert
            Assert.Equal(result, "https://api.twitter.com/1.1/favorites/list.json?user_id=42&include_entities=true&since_id=43&max_id=42&count=12&tweet_mode=extended&hello=world");

            _fakeUserQueryValidator.CallsTo(x => x.ThrowIfUserCannotBeIdentified(user)).MustHaveHappened();
        }
    }
}