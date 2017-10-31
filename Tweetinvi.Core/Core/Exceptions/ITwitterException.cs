using System;
using System.Collections.Generic;
using System.Net;
using Tweetinvi.Models;

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

        /// <summary>
        /// The credentials used to make the request that gave this exception
        /// </summary>
        ITwitterCredentials Credentials { get; }
    }
}