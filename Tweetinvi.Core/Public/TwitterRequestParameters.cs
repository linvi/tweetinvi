using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Tweetinvi.Models;
using HttpMethod = Tweetinvi.Models.HttpMethod;

namespace Tweetinvi
{
   public class TwitterRequestParameters : ITwitterRequestParameters
    {
        public TwitterRequestParameters()
        {
        }

        public TwitterRequestParameters(ITwitterRequestParameters source)
        {
            if (source == null)
            {
                return;
            }

            Url = source.Url;
            HttpMethod = source.HttpMethod;
            AcceptHeaders = source.AcceptHeaders.ToList();
            CustomHeaders = source.CustomHeaders.ToDictionary(x => x.Key, x => x.Value);
            AuthorizationHeader = source.AuthorizationHeader;
        }
        
        public string Url { get; set; }
        public HttpMethod HttpMethod { get; set; }
        public virtual HttpContent HttpContent { get; set; }
        public bool IsHttpContentPartOfQueryParams { get; set; }
        public List<string> AcceptHeaders { get; set; }
        public Dictionary<string, string> CustomHeaders { get; set; }
        public string AuthorizationHeader { get; set; }
    }
}
