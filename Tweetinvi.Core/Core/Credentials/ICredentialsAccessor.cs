using System;
using Tweetinvi.Models;

namespace Tweetinvi.Core.Credentials
{
    public interface ICredentialsAccessor
    {
        ITwitterCredentials ApplicationCredentials { get; set; }
        ITwitterCredentials CurrentThreadCredentials { get; set; }
    }
}