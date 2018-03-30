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
        /// Binary to be send via HttpRequest
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
            }
        }
        public string ContentId { get; set; }

        public Action<long, long> UploadProgressChanged { get; set; }

        public override HttpContent HttpContent
        {
            get { return GetMultipartFormDataContent(ContentId, _binaries); }
            set { throw new InvalidOperationException("Multipart HttpContent is created based on the binaries of the MultipartRequest.");}
        }

        private ProgressableStreamContent GetMultipartFormDataContent(string contentId, IEnumerable<byte[]> binaries)
        {
            var multiPartContent = new MultipartFormDataContent();

            int i = 0;
            foreach (var binary in binaries)
            {
                var byteArrayContent = new ByteArrayContent(binary);
                byteArrayContent.Headers.Add("Content-Type", "application/octet-stream");
                multiPartContent.Add(byteArrayContent, contentId, i.ToString(CultureInfo.InvariantCulture));
            }

            var progressableContent = new ProgressableStreamContent(multiPartContent, (current, total) =>
            {
                UploadProgressChanged?.Invoke(current, total);
            });

            return progressableContent;
        }
    }
}