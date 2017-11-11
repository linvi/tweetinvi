using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using HttpMethod = Tweetinvi.Models.HttpMethod;

namespace Tweetinvi.Core.Web
{
    public interface IMultipartHttpRequestParameters : IHttpRequestParameters
    {
        /// <summary>
        /// Binaries to be send via HttpRequest
        /// </summary>
        IEnumerable<byte[]> Binaries { get; set; }

        /// <summary>
        /// Content Id
        /// </summary>
        string ContentId { get; set; }

        /// <summary>
        /// Action invoked to show the progress of the upload. {current / total}
        /// </summary>
        Action<long, long> UploadProgressChanged { get; set; }
    }

    public class MultipartHttpRequestParameters : HttpRequestParameters, IMultipartHttpRequestParameters
    {
        private IEnumerable<byte[]> _binaries;
        private HttpContent _httpContent;

        public MultipartHttpRequestParameters()
        {
            ContentId = "media";
            HttpMethod = HttpMethod.POST;
        }

        public IEnumerable<byte[]> Binaries
        {
            get { return _binaries; }
            set
            {
                _binaries = value;

                var multipartFormDataContent = GetMultipartFormDataContent(ContentId, _binaries);

                var progressableContent = new ProgressableStreamContent(multipartFormDataContent, (current, total) =>
                {
                    UploadProgressChanged?.Invoke(current, total);
                });

                _httpContent = progressableContent;
            }
        }
        public string ContentId { get; set; }

        public Action<long, long> UploadProgressChanged { get; set; }

        public override HttpContent HttpContent
        {
            get { return _httpContent; }
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