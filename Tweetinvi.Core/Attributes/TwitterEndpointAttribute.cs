using System;

namespace Tweetinvi.Core.Attributes
{
    public interface ITwitterEndpointAttribute
    {
        /// <summary>
        /// Endpoint url matcher.
        /// </summary>
        string EndpointURL { get; }

        /// <summary>
        /// Specify the the EndpointURL parameter is a regex.
        /// </summary>
        bool IsRegex { get; }
    }

    /// <summary>
    /// Attribute indicating how to match an endpoint rate limits with a specific url
    /// as well as the number of available requests for this endpoint.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class TwitterEndpointAttribute : Attribute, ITwitterEndpointAttribute
    {
        public TwitterEndpointAttribute(string endpointURL, bool isRegex = false)
        {
            EndpointURL = endpointURL;
            IsRegex = isRegex;
        }

        public string EndpointURL { get; private set; }
        public bool IsRegex { get; private set; }
    }
}