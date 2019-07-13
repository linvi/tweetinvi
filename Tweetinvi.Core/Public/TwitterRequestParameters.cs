using System.Collections.Generic;
using System.Net.Http;
using Tweetinvi.Models;
using HttpMethod = Tweetinvi.Models.HttpMethod;

namespace Tweetinvi
{
   public class TwitterRequestParameters : ITwitterRequestParameters
    {
        public string Url { get; set; }
        public HttpMethod HttpMethod { get; set; }
        public HttpContent HttpContent { get; set; }
        public List<string> AcceptHeaders { get; set; }
        public Dictionary<string, string> CustomHeaders { get; set; }
        public string AuthorizationHeader { get; set; }
    }
}
