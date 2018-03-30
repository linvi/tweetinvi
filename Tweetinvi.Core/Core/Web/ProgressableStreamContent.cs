using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Tweetinvi.Core.Web
{
    // This code has been reusing the following code : https://stackoverflow.com/questions/41378457/c-httpclient-file-upload-progress-when-uploading-multiple-file-as-multipartfo

    public class ProgressableStreamContent : HttpContent
    {
        private const int DEFAULT_BUFFER_SIZE = 5 * 4096;

        private HttpContent _content;
        private int _bufferSize;
        private Action<long, long> _progress;

        public ProgressableStreamContent(HttpContent content, Action<long, long> progress) : this(content, DEFAULT_BUFFER_SIZE, progress) { }

        public ProgressableStreamContent(HttpContent content, int bufferSize, Action<long, long> progress)
        {
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }

            if (bufferSize <= 0)
            {
                throw new ArgumentOutOfRangeException("bufferSize");
            }

            _content = content;
            _bufferSize = bufferSize;
            _progress = progress;

            foreach (var h in content.Headers)
            {
                Headers.Add(h.Key, h.Value);
            }
        }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            return Task.Run(async () =>
            {
                var buffer = new Byte[_bufferSize];
                long uploaded = 0;

                TryComputeLength(out var size);

                using (var contentStream = await _content.ReadAsStreamAsync())
                {
                    while (true)
                    {
                        var length = contentStream.Read(buffer, 0, buffer.Length);
                        if (length <= 0) break;

                        uploaded += length;
                        _progress?.Invoke(uploaded, size);

                        stream.Write(buffer, 0, length);
                        stream.Flush();
                    }
                }

                stream.Flush();
            });
        }

        protected override bool TryComputeLength(out long length)
        {
            length = _content.Headers.ContentLength.GetValueOrDefault();
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _content.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}