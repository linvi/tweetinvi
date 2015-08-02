using Microsoft.Practices.Unity;
using TweetinviCore.Wrappers;

namespace TweetinviLogic.Wrapper
{
    public class ParameterOverrideWrapper : IParameterOverrideWrapper
    {
        public object ResolverOverride
        {
            get { return new ParameterOverride(ParameterName, ParameterValue); }
        }
        public string ParameterName { get; set; }
        public object ParameterValue { get; set; }
    }
}