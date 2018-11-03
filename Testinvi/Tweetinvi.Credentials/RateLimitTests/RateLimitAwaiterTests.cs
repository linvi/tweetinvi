using System;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.RateLimit;
using Tweetinvi.Credentials.RateLimit;
using Tweetinvi.Events;
using Tweetinvi.Models;

namespace Testinvi.Tweetinvi.Credentials.RateLimitTests
{
    [TestClass]
    public class RateLimitAwaiterTests
    {
        private const string TEST_QUERY = "my test query";
        private readonly int TIME_TO_WAIT = new Random(Int32.MaxValue).Next();

        private FakeClassBuilder<RateLimitAwaiter> _fakeBuilder;
        
        private WeakEvent<EventHandler<QueryAwaitingEventArgs>> _weakEvent;

        private IRateLimitCacheManager _rateLimitCacheManager;
        private ICredentialsAccessor _credentialsAccessor;
        private IThreadHelper _threadHelper;
        private ITwitterCredentials _credentials;
        private IEndpointRateLimit _endpointRateLimit;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<RateLimitAwaiter>();
            _rateLimitCacheManager = _fakeBuilder.GetFake<IRateLimitCacheManager>().FakedObject;
            _credentialsAccessor = _fakeBuilder.GetFake<ICredentialsAccessor>().FakedObject;
            _threadHelper = _fakeBuilder.GetFake<IThreadHelper>().FakedObject;

            _credentials = A.Fake<ITwitterCredentials>();
            _endpointRateLimit = A.Fake<IEndpointRateLimit>();
            A.CallTo(() => _endpointRateLimit.Remaining).Returns(0);
            A.CallTo(() => _endpointRateLimit.ResetDateTimeInMilliseconds).Returns(TIME_TO_WAIT);

            A.CallTo(() => _rateLimitCacheManager.GetQueryRateLimit(TEST_QUERY, _credentials))
                .Returns(_endpointRateLimit);
            A.CallTo(() => _credentialsAccessor.CurrentThreadCredentials).Returns(_credentials);
        }

        [TestMethod]
        public void WaitForCurrentCredentialsRateLimit_AwaitForRateLimitCheckerTimeToWait()
        {
            // Arrange
            var rateLimitAwaiter = CreateRateLimitAwaiter();

            // Act
            rateLimitAwaiter.WaitForCurrentCredentialsRateLimit(TEST_QUERY);

            // Assert
            A.CallTo(() => _threadHelper.Sleep(TIME_TO_WAIT)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [TestMethod]
        public void WaitForRateLimit_AwaitForRateLimitCheckerTimeToWait()
        {
            // Arrange
            var rateLimitAwaiter = CreateRateLimitAwaiter();

            // Act
            rateLimitAwaiter.WaitForCredentialsRateLimit(TEST_QUERY, _credentials);

            // Assert
            A.CallTo(() => _threadHelper.Sleep(TIME_TO_WAIT)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [TestMethod]
        public void WaitForRateLimit_EventRaised()
        {
            // Arrange
            var rateLimitAwaiter = CreateRateLimitAwaiter();
            
            var eventTestHelper = new EventTestHelper<QueryAwaitingEventArgs>();
            rateLimitAwaiter.QueryAwaitingForRateLimit += eventTestHelper.EventAction;

            // Act
            rateLimitAwaiter.WaitForCredentialsRateLimit(TEST_QUERY, _credentials);

            // Assert
            eventTestHelper.VerifyNumberOfCalls(1);
            eventTestHelper.VerifyAtWhere(0, x => x.Credentials == _credentials);
        }

        [TestMethod]
        public void WaitForRateLimit_Unregister_EventRaised()
        {
            // Arrange
            var rateLimitAwaiter = CreateRateLimitAwaiter();

            var eventTestHelper = new EventTestHelper<QueryAwaitingEventArgs>();
            rateLimitAwaiter.QueryAwaitingForRateLimit += eventTestHelper.EventAction;
            rateLimitAwaiter.QueryAwaitingForRateLimit -= eventTestHelper.EventAction;

            // Act
            rateLimitAwaiter.WaitForCredentialsRateLimit(TEST_QUERY, _credentials);

            // Assert
            eventTestHelper.VerifyNumberOfCalls(0);
        }

        [TestMethod]
        public void WaitForRateLimit_QueriesCanBePerformedRightAway_DoNotWait()
        {
            // Arrange
            var rateLimitAwaiter = CreateRateLimitAwaiter();
            A.CallTo(() => _endpointRateLimit.Remaining).Returns(15);

            // Act
            rateLimitAwaiter.WaitForCredentialsRateLimit(TEST_QUERY, _credentials);

            // Assert
            A.CallTo(() => _threadHelper.Sleep(It.IsAny<int>())).MustNotHaveHappened();
        }

        [TestMethod]
        public void WaitForRateLimit_TokenCannotBeAssociatedWithQuery_DoNotWait()
        {
            // Arrange
            var rateLimitAwaiter = CreateRateLimitAwaiter();
            A.CallTo(() => _rateLimitCacheManager.GetQueryRateLimit(TEST_QUERY, _credentials)).Returns(null);

            // Act
            rateLimitAwaiter.WaitForCredentialsRateLimit(TEST_QUERY, _credentials);

            // Assert
            A.CallTo(() => _threadHelper.Sleep(It.IsAny<int>())).MustNotHaveHappened();
        }

        [TestMethod]
        public void TimeToWaitBeforeTwitterRequest_RemainingQueries_Returns0()
        {
            // Arrange
            var rateLimitAwaiter = CreateRateLimitAwaiter();
            A.CallTo(() => _endpointRateLimit.Remaining).Returns(9);

            // Act
            var result = rateLimitAwaiter.TimeToWaitBeforeTwitterRequest(TEST_QUERY, _credentials);

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void TimeToWaitBeforeTwitterRequest_NoRemainingQueries_ReturnsTimeToWait()
        {
            // Arrange
            var rateLimitAwaiter = CreateRateLimitAwaiter();

            // Act
            var result = rateLimitAwaiter.TimeToWaitBeforeTwitterRequest(TEST_QUERY, _credentials);

            // Assert
            Assert.AreEqual(TIME_TO_WAIT, result);
        }

        private RateLimitAwaiter CreateRateLimitAwaiter()
        {
            _weakEvent = new WeakEvent<EventHandler<QueryAwaitingEventArgs>>();
            return _fakeBuilder.GenerateClass(new ConstructorNamedParameter("queryAwaitingForRateLimitWeakEvent", _weakEvent));
        }
    }
}