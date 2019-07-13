using System;
using System.Threading.Tasks;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
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
        // ReSharper disable once InconsistentNaming
        private readonly int TIME_TO_WAIT = new Random(int.MaxValue).Next();

        private FakeClassBuilder<RateLimitAwaiter> _fakeBuilder;
        private Fake<IRateLimitCacheManager> _fakeRateLimitCacheManager;
        private Fake<ICredentialsAccessor> _fakeCredentialsAccessor;
        private Fake<IThreadHelper> _fakeThreadHelper;
        private WeakEvent<EventHandler<QueryAwaitingEventArgs>> _weakEvent;

        private ITwitterCredentials _credentials;
        private IEndpointRateLimit _endpointRateLimit;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<RateLimitAwaiter>();
            _fakeRateLimitCacheManager = _fakeBuilder.GetFake<IRateLimitCacheManager>();
            _fakeCredentialsAccessor = _fakeBuilder.GetFake<ICredentialsAccessor>();
            _fakeThreadHelper = _fakeBuilder.GetFake<IThreadHelper>();

            _credentials = A.Fake<ITwitterCredentials>();
            _endpointRateLimit = A.Fake<IEndpointRateLimit>();
            _endpointRateLimit.CallsTo(x => x.Remaining).Returns(0);
            _endpointRateLimit.CallsTo(x => x.ResetDateTimeInMilliseconds).Returns(TIME_TO_WAIT);

            _fakeRateLimitCacheManager.CallsTo(x => x.GetQueryRateLimit(TEST_QUERY, _credentials)).Returns(_endpointRateLimit);
            _fakeCredentialsAccessor.CallsTo(x => x.CurrentThreadCredentials).Returns(_credentials);
        }

        [TestMethod]
        public async Task WaitForCurrentCredentialsRateLimit_AwaitForRateLimitCheckerTimeToWait()
        {
            // Arrange
            var rateLimitAwaiter = CreateRateLimitAwaiter();

            // Act
            await rateLimitAwaiter.WaitForCurrentCredentialsRateLimit(TEST_QUERY);

            // Assert
            _fakeThreadHelper.CallsTo(x => x.Sleep(TIME_TO_WAIT)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [TestMethod]
        public async Task WaitForRateLimit_AwaitForRateLimitCheckerTimeToWait()
        {
            // Arrange
            var rateLimitAwaiter = CreateRateLimitAwaiter();

            // Act
            await rateLimitAwaiter.WaitForCredentialsRateLimit(TEST_QUERY, _credentials);

            // Assert
            _fakeThreadHelper.CallsTo(x => x.Sleep(TIME_TO_WAIT)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [TestMethod]
        public async Task WaitForRateLimit_EventRaised()
        {
            // Arrange
            var rateLimitAwaiter = CreateRateLimitAwaiter();

            var eventTestHelper = new EventTestHelper<QueryAwaitingEventArgs>();
            rateLimitAwaiter.QueryAwaitingForRateLimit += eventTestHelper.EventAction;

            // Act
            await rateLimitAwaiter.WaitForCredentialsRateLimit(TEST_QUERY, _credentials);

            // Assert
            eventTestHelper.VerifyNumberOfCalls(1);
            eventTestHelper.VerifyAtWhere(0, x => x.Credentials == _credentials);
        }

        [TestMethod]
        public async Task WaitForRateLimit_Unregister_EventRaised()
        {
            // Arrange
            var rateLimitAwaiter = CreateRateLimitAwaiter();

            var eventTestHelper = new EventTestHelper<QueryAwaitingEventArgs>();
            rateLimitAwaiter.QueryAwaitingForRateLimit += eventTestHelper.EventAction;
            rateLimitAwaiter.QueryAwaitingForRateLimit -= eventTestHelper.EventAction;

            // Act
            await rateLimitAwaiter.WaitForCredentialsRateLimit(TEST_QUERY, _credentials);

            // Assert
            eventTestHelper.VerifyNumberOfCalls(0);
        }

        [TestMethod]
        public async Task WaitForRateLimit_QueriesCanBePerformedRightAway_DoNotWait()
        {
            // Arrange
            var rateLimitAwaiter = CreateRateLimitAwaiter();
            _endpointRateLimit.CallsTo(x => x.Remaining).Returns(15);

            // Act
            await rateLimitAwaiter.WaitForCredentialsRateLimit(TEST_QUERY, _credentials);

            // Assert
            _fakeThreadHelper.CallsTo(x => x.Sleep(It.IsAny<int>())).MustNotHaveHappened();
        }

        [TestMethod]
        public async Task WaitForRateLimit_TokenCannotBeAssociatedWithQuery_DoNotWait()
        {
            // Arrange
            var rateLimitAwaiter = CreateRateLimitAwaiter();
            _fakeRateLimitCacheManager.CallsTo(x => x.GetQueryRateLimit(TEST_QUERY, _credentials)).Returns(Task.FromResult((IEndpointRateLimit)null));

            // Act
            await rateLimitAwaiter.WaitForCredentialsRateLimit(TEST_QUERY, _credentials);

            // Assert
            _fakeThreadHelper.CallsTo(x => x.Sleep(It.IsAny<int>())).MustNotHaveHappened();
        }

        [TestMethod]
        public async Task TimeToWaitBeforeTwitterRequest_RemainingQueries_Returns0()
        {
            // Arrange
            var rateLimitAwaiter = CreateRateLimitAwaiter();
            _endpointRateLimit.CallsTo(x => x.Remaining).Returns(9);

            // Act
            var result = await rateLimitAwaiter.TimeToWaitBeforeTwitterRequest(TEST_QUERY, _credentials);

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public async Task TimeToWaitBeforeTwitterRequest_NoRemainingQueries_ReturnsTimeToWait()
        {
            // Arrange
            var rateLimitAwaiter = CreateRateLimitAwaiter();

            // Act
            var result = await rateLimitAwaiter.TimeToWaitBeforeTwitterRequest(TEST_QUERY, _credentials);

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