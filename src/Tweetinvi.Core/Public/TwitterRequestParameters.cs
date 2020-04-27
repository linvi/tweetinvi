using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Tweetinvi.Core.Extensions;
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
            CustomHeaders = new CustomRequestHeaders();
            source.CustomHeaders.ForEach(customHeader =>
            {
               CustomHeaders.Add(customHeader.Key, customHeader.Values, customHeader.Behaviour);
            });

            AuthorizationHeader = source.AuthorizationHeader;
        }

        public string Url { get; set; }
        public HttpMethod HttpMethod { get; set; }
        public virtual HttpContent HttpContent { get; set; }
        public bool IsHttpContentPartOfQueryParams { get; set; }
        public List<string> AcceptHeaders { get; set; }
        public string AuthorizationHeader { get; set; }
        public CustomRequestHeaders CustomHeaders { get; set; }
    }
}
