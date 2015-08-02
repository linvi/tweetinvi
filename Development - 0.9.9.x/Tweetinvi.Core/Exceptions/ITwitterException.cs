using System;
using System.Collections.Generic;
using System.Net;

namespace Tweetinvi.Core.Exceptions
{
    public interface ITwitterException
    {
        WebException WebException { get; }

        string URL { get; }
        int StatusCode { get; }
        string TwitterDescription { get; }
        DateTime CreationDate { get; }
        IEnumerable<ITwitterExceptionInfo> TwitterExceptionInfos { get; }
    }
}