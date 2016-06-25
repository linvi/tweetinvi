using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using HttpMethod = Tweetinvi.Models.HttpMethod;

namespace Tweetinvi.Core.Web
{
    public interface IMultipartHttpRequestParameters : IHttpRequestParameters
    {
        IEnumerable<byte[]> Binaries { get; set; }
        string ContentId { get; set; }
    }

    public class MultipartHttpRequestParameters : HttpRequestParameters, IMultipartHttpRequestParameters
    {
        public MultipartHttpRequestParameters()
        {
            ContentId = "media";
            HttpMethod = HttpMethod.POST;
        }

        public IEnumerable<byte[]> Binaries { get; set; }
        public string ContentId { get; set; }

        public override HttpContent HttpContent
        {
            get { return GetMultipartFormDataContent(ContentId, Binaries); }
            set { throw new InvalidOperationException("Multipart HttpContent is created based on the binaries of the MultipartRequest.");}
        }

        private MultipartFormDataContent GetMultipartFormDataContent(string contentId, IEnumerable<byte[]> binaries)
        {
            var multiPartContent = new MultipartFormDataContent();

            int i = 0;
            foreach (var binary in binaries)
            {
                var byteArrayContent = new ByteArrayContent(binary);
                byteArrayContent.Headers.Add("Content-Type", "application/octet-stream");
                multiPartContent.Add(byteArrayContent, contentId, i.ToString(CultureInfo.InvariantCulture));
            }

            return multiPartContent;
        }
    }
}