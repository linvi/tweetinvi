using System;

namespace Tweetinvi.Core.Attributes
{
    public interface ICustomTwitterEndpointAttribute
    {
        string EndpointURL { get; }
        int NumberOfExpectedCalls { get; }
    }

    /// <summary>
    /// Annotate method to make it available for the RateLimitAccessor
    /// </summary>
    public class CustomTwitterEndpointAttribute : Attribute, ICustomTwitterEndpointAttribute
    {
        public CustomTwitterEndpointAttribute(string endpointURL, int numberOfExpectedCalls = 1)
        {
            EndpointURL = endpointURL;
            NumberOfExpectedCalls = numberOfExpectedCalls;
        }

        public string EndpointURL { get; private set; }
        public int NumberOfExpectedCalls { get; private set; }
    }
}