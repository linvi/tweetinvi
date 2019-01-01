using System;
using System.Diagnostics.CodeAnalysis;
using FakeItEasy;
using Testinvi.Helpers;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Models;

namespace Testinvi.SetupHelpers
{
    [ExcludeFromCodeCoverage]
    public static class CredentialsRunnerHelper
    {
        public static void SetupPassThrough<T>(this Fake<ICredentialsRunner> fakeCredentialsRunner) where T : class 
        {
            fakeCredentialsRunner
                .CallsTo(x => x.ExecuteOperationWithCredentials(It.IsAny<ITwitterCredentials>(), It.IsAny<Func<T>>()))
                .ReturnsLazily((ITwitterCredentials cred, Func<T> f) => f());
        }
    }
}
