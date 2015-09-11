using System;

namespace Tweetinvi.Core.Attributes
{
    public interface ITwitterEndpointAttribute
    {
        string EndpointURL { get; }
        bool IsRegex { get; }
        int NumberOfRequests { get; }
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class TwitterEndpointAttribute : Attribute, ITwitterEndpointAttribute
    {
        public TwitterEndpointAttribute(string endpointURL, bool isRegex = false, int numberOfRequests = 1)
        {
            EndpointURL = endpointURL;
            IsRegex = isRegex;
            NumberOfRequests = numberOfRequests;
        }

        public string EndpointURL { get; private set; }
        public bool IsRegex { get; private set; }
        public int NumberOfRequests { get; private set; }
    }
}