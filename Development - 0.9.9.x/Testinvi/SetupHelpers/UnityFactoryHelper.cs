using System.Diagnostics.CodeAnalysis;
using FakeItEasy;
using Testinvi.Helpers;
using Tweetinvi.Core.Injectinvi;

namespace Testinvi.SetupHelpers
{
    [ExcludeFromCodeCoverage]
    public static class UnityFactoryHelper
    {
        public static void ArrangeGenerateParameterOverride<T, U>(this Fake<IFactory<U>> fakeFactory, IConstructorNamedParameter parameter = null)
        {
            fakeFactory
                .CallsTo(x => x.GenerateParameterOverrideWrapper(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsLazily((string paramName, object paramValue) =>
                {
                    if (parameter != null)
                    {
                        return parameter;
                    }

                    var fakeParameter = new Fake<IConstructorNamedParameter>();

                    fakeParameter.CallsTo(x => x.Name).Returns(paramName);
                    fakeParameter.CallsTo(x => x.Value).Returns(paramValue);

                    return fakeParameter.FakedObject;
                });
        }
    }
}