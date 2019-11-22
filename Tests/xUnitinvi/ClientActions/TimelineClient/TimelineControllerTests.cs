using System;
using System.Threading.Tasks;
using FakeItEasy;
using Tweetinvi.Controllers.Timeline;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;
using Xunit;
using xUnitinvi.TestHelpers;

namespace xUnitinvi.ClientActions.TimelineClient
{
    public class TimelineControllerTests
    {
        public TimelineControllerTests()
        {
            _fakeBuilder = new FakeClassBuilder<TimelineController>();
            _fakePageCursorIteratorFactories = _fakeBuilder.GetFake<IPageCursorIteratorFactories>().FakedObject;
        }

        private readonly FakeClassBuilder<TimelineController> _fakeBuilder;
        private readonly IPageCursorIteratorFactories _fakePageCursorIteratorFactories;

        private TimelineController CreateTimelineController()
        {
            return _fakeBuilder.GenerateClass();
        }

        [Fact]
        public void GetHomeTimelineIterator_ReturnsFromPageCursorIteratorFactories()
        {
            // arrange
            var parameters = new GetHomeTimelineParameters { PageSize = 2 };
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterPageIterator<ITwitterResult<ITweetDTO[]>, long?>>();

            A.CallTo(() => _fakePageCursorIteratorFactories.Create(parameters, It.IsAny<Func<long?, Task<ITwitterResult<ITweetDTO[]>>>>()))
                .Returns(expectedResult);

            var controller = CreateTimelineController();
            var iterator = controller.GetHomeTimelineIterator(parameters, request);

            // assert
            Assert.Equal(iterator, expectedResult);
        }

        [Fact]
        public void GetMentionsTimelineIterator_ReturnsFromPageCursorIteratorFactories()
        {
            // arrange
            var parameters = new GetMentionsTimelineParameters { PageSize = 2 };
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterPageIterator<ITwitterResult<ITweetDTO[]>, long?>>();

            A.CallTo(() => _fakePageCursorIteratorFactories.Create(parameters, It.IsAny<Func<long?, Task<ITwitterResult<ITweetDTO[]>>>>()))
                .Returns(expectedResult);

            var controller = CreateTimelineController();
            var iterator = controller.GetMentionsTimelineIterator(parameters, request);

            // assert
            Assert.Equal(iterator, expectedResult);
        }

        [Fact]
        public void GetUserTimelineIterator_ReturnsFromPageCursorIteratorFactories()
        {
            // arrange
            var parameters = new GetUserTimelineParameters("linvi") { PageSize = 2 };
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterPageIterator<ITwitterResult<ITweetDTO[]>, long?>>();

            A.CallTo(() => _fakePageCursorIteratorFactories.Create(parameters, It.IsAny<Func<long?, Task<ITwitterResult<ITweetDTO[]>>>>()))
                .Returns(expectedResult);

            var controller = CreateTimelineController();
            var iterator = controller.GetUserTimelineIterator(parameters, request);

            // assert
            Assert.Equal(iterator, expectedResult);
        }

        [Fact]
        public void GetRetweetsOfMeTimelineIterator_ReturnsFromPageCursorIteratorFactories()
        {
            // arrange
            var parameters = new GetRetweetsOfMeTimelineParameters { PageSize = 2 };
            var request = A.Fake<ITwitterRequest>();
            var expectedResult = A.Fake<ITwitterPageIterator<ITwitterResult<ITweetDTO[]>, long?>>();

            A.CallTo(() => _fakePageCursorIteratorFactories.Create(parameters, It.IsAny<Func<long?, Task<ITwitterResult<ITweetDTO[]>>>>()))
                .Returns(expectedResult);

            var controller = CreateTimelineController();
            var iterator = controller.GetRetweetsOfMeTimelineIterator(parameters, request);

            // assert
            Assert.Equal(iterator, expectedResult);
        }
    }
}