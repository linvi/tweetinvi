using System;
using System.Globalization;
using System.Net.Http;
using Tweetinvi.Core.Upload;
using Tweetinvi.Events;
using Tweetinvi.Models;
using HttpMethod = Tweetinvi.Models.HttpMethod;

namespace Tweetinvi
{
    public interface IMultipartTwitterQuery : ITwitterQuery
    {
        /// <summary>
        /// Binary to be send via HttpRequest
        /// </summary>
        byte[][] Binaries { get; set; }

        /// <summary>
        /// Content Id
        /// </summary>
        string ContentId { get; set; }

        /// <summary>
        /// Action invoked to show the progress of the upload. {current / total}
        /// </summary>
        Action<IUploadProgressChanged> UploadProgressChanged { get; set; }
    }

    public class MultipartTwitterQuery : TwitterQuery, IMultipartTwitterQuery
    {
        private byte[][] _binaries;

        public MultipartTwitterQuery()
        {
            ContentId = "media";
            HttpMethod = HttpMethod.POST;
        }

        public MultipartTwitterQuery(ITwitterQuery source) : base(source)
        {
            ContentId = "media";
            HttpMethod = HttpMethod.POST;
        }
        
        public MultipartTwitterQuery(IMultipartTwitterQuery source) : base(source)
        {
            if (source == null)
            {
                ContentId = "media";
                HttpMethod = HttpMethod.POST;
                return;
            }

            _binaries = source.Binaries;
            ContentId = source.ContentId;
            UploadProgressChanged = source.UploadProgressChanged;
        }

        public byte[][] Binaries
        {
            get => _binaries;
            set => _binaries = value;
        }

        public string ContentId { get; set; }

        public Action<IUploadProgressChanged> UploadProgressChanged { get; set; }

        public override HttpContent HttpContent
        {
            get => GetMultipartFormDataContent(ContentId, _binaries);
            set => throw new InvalidOperationException("Multipart HttpContent is created based on the binaries of the MultipartRequest.");
        }

        private ProgressableStreamContent GetMultipartFormDataContent(string contentId, byte[][] binaries)
        {
            var multiPartContent = CreateHttpContent(contentId, binaries);

            var progressableContent = new ProgressableStreamContent(multiPartContent, (args) =>
            {
                UploadProgressChanged?.Invoke(args);
            });

            return progressableContent;
        }

        public static MultipartFormDataContent CreateHttpContent(string contentId, byte[][] binaries)
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