using System;
using System.IO;
using System.Net;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Wrappers;
using Tweetinvi.Logic.Properties;

namespace Tweetinvi.Logic.Exceptions
{
    public class WebExceptionInfoExtractor : IWebExceptionInfoExtractor
    {
        private readonly IJObjectStaticWrapper _jObjectStaticWrapper;
        private readonly IFactory<ITwitterExceptionInfo> _twitterExceptionInfoUnityFactory;

        public WebExceptionInfoExtractor(
            IJObjectStaticWrapper jObjectStaticWrapper,
            IFactory<ITwitterExceptionInfo> twitterExceptionInfoUnityFactory)
        {
            _jObjectStaticWrapper = jObjectStaticWrapper;
            _twitterExceptionInfoUnityFactory = twitterExceptionInfoUnityFactory;
        }

        public int GetWebExceptionStatusNumber(WebException wex)
        {
            return GetWebExceptionStatusNumber(wex, -1);
        }

        public int GetWebExceptionStatusNumber(WebException wex, int defaultStatusCode)
        {
            if (wex.Response is HttpWebResponse wexResponse)
            {
                return (int) wexResponse.StatusCode;
            }

            return defaultStatusCode;
        }

        public string GetStatusCodeDescription(int statusCode)
        {
            return Resources.GetResourceByName($"ExceptionDescription_{statusCode}");
        }

        public ITwitterExceptionInfo[] GetTwitterExceptionInfos(string content)
        {
            try
            {
                var jObject = _jObjectStaticWrapper.GetJobjectFromJson(content);
                return _jObjectStaticWrapper.ToObject<ITwitterExceptionInfo[]>(jObject["errors"]);
            }
            catch (Exception)
            {
                var twitterInfo = _twitterExceptionInfoUnityFactory.Create();
                twitterInfo.Message = content;
                return new[] {twitterInfo};
            }
        }

        public ITwitterExceptionInfo[] GetTwitterExceptionInfos(WebException wex)
        {
            var wexResponse = wex.Response as HttpWebResponse;

            if (wexResponse == null)
            {
                return new ITwitterExceptionInfo[0];
            }

            try
            {
                return GetStreamInfo(wexResponse);
            }
            catch (WebException)
            {
            }

            return new ITwitterExceptionInfo[0];
        }

        private ITwitterExceptionInfo[] GetStreamInfo(HttpWebResponse wexResponse)
        {
            using (var stream = wexResponse.GetResponseStream())
            {
                return GetTwitterExceptionInfosFromStream(stream);
            }
        }

        public ITwitterExceptionInfo[] GetTwitterExceptionInfosFromStream(Stream stream)
        {
            if (stream == null)
            {
                return null;
            }

            using (var reader = new StreamReader(stream))
            {
                var content = reader.ReadToEnd();
                return GetTwitterExceptionInfos(content);
            }
        }
    }
}