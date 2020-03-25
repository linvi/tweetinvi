using System;
using System.Net.Http;
using System.Threading.Tasks;
using FakeItEasy;
using Newtonsoft.Json;
using Tweetinvi.Core.Models.Properties;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using HttpMethod = Tweetinvi.Models.HttpMethod;

namespace xUnitinvi.TestHelpers
{
    public class TwitterAccessorSpy : Fake<ITwitterAccessor>, ITwitterAccessor
    {
        private readonly ITwitterAccessor _twitterAccessor;

        public TwitterAccessorSpy(ITwitterAccessor twitterAccessor)
        {
            _twitterAccessor = twitterAccessor;
        }

        public Task<ITwitterResult> ExecuteRequest(ITwitterRequest request)
        {
            A.CallTo(() => FakedObject.ExecuteRequest(request))
                .ReturnsLazily((ITwitterRequest requestPassed) => _twitterAccessor.ExecuteRequest(requestPassed));

            return FakedObject.ExecuteRequest(request);
        }

        public Task<ITwitterResult<T>> ExecuteRequest<T>(ITwitterRequest request) where T : class
        {
            A.CallTo(() => FakedObject.ExecuteRequest<T>(request))
                .ReturnsLazily((ITwitterRequest requestPassed) => _twitterAccessor.ExecuteRequest<T>(requestPassed));

            return FakedObject.ExecuteRequest<T>(request);
        }

        public Task<byte[]> DownloadBinary(ITwitterRequest request)
        {
            throw new NotImplementedException();
        }

        public Task PrepareTwitterRequest(ITwitterRequest request)
        {
            throw new NotImplementedException();
        }
    }
}