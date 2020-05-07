using System;
using System.Threading.Tasks;
using FakeItEasy;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;

namespace xUnitinvi.TestHelpers
{
    public class TwitterAccessorSpy : Fake<ITwitterAccessor>, ITwitterAccessor
    {
        private readonly ITwitterAccessor _twitterAccessor;

        public TwitterAccessorSpy(ITwitterAccessor twitterAccessor)
        {
            _twitterAccessor = twitterAccessor;
        }

        public Task<ITwitterResult> ExecuteRequestAsync(ITwitterRequest request)
        {
            A.CallTo(() => FakedObject.ExecuteRequestAsync(request))
                .ReturnsLazily((ITwitterRequest requestPassed) => _twitterAccessor.ExecuteRequestAsync(requestPassed));

            return FakedObject.ExecuteRequestAsync(request);
        }

        public Task<ITwitterResult<T>> ExecuteRequestAsync<T>(ITwitterRequest request) where T : class
        {
            A.CallTo(() => FakedObject.ExecuteRequestAsync<T>(request))
                .ReturnsLazily((ITwitterRequest requestPassed) => _twitterAccessor.ExecuteRequestAsync<T>(requestPassed));

            return FakedObject.ExecuteRequestAsync<T>(request);
        }

        public Task<byte[]> DownloadBinaryAsync(ITwitterRequest request)
        {
            throw new NotImplementedException();
        }

        public Task PrepareTwitterRequestAsync(ITwitterRequest request)
        {
            throw new NotImplementedException();
        }
    }
}