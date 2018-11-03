using System.Diagnostics.CodeAnalysis;
using FakeItEasy;
using Testinvi.Helpers;
using Tweetinvi.Core.Injectinvi;

namespace Testinvi.SetupHelpers
{
    [ExcludeFromCodeCoverage]
    public static class UnityFactoryHelper
    {
        public static void ArrangeGenerateParameterOverride<T, U>(this IFactory<U> factory, IConstructorNamedParameter parameter = null)
        {
            A.CallTo(() => factory.GenerateParameterOverrideWrapper(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsLazily((string paramName, object paramValue) =>
                {
                    if (parameter != null)
                    {
                        return parameter;
                    }

                    var param = A.Fake<IConstructorNamedParameter>();

                    A.CallTo(() => param.Name).Returns(paramName);
                    A.CallTo(() => param.Value).Returns(paramValue);

                    return param;
                });
        }
    }
}