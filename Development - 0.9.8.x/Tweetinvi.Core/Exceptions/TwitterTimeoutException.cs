using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Tweetinvi.Logic.Exceptions;

namespace Tweetinvi.Core.Exceptions
{
    public interface ITwitterTimeoutException : ITwitterException
    {
    }

    public class TwitterTimeoutException : TwitterException, ITwitterTimeoutException
    {
        public TwitterTimeoutException(ITweetinviSettingsAccessor tweetinviSettingsAccessor, string url) : base(url, string.Format("{0} web request timed out.", url))
        {
            TwitterDescription = string.Format("Twitter was not able to perform your query within the Timeout limit of {0} ms.", tweetinviSettingsAccessor.WebRequestTimeout);
            CreationDate = DateTime.Now;
        }

        public override WebException WebException { get { return null; } }
        public override int StatusCode { get { return 408; } }
        public override IEnumerable<ITwitterExceptionInfo> TwitterExceptionInfos
        {
            get
            {
                return Enumerable.Empty<ITwitterExceptionInfo>();
            }
        }
    }
}