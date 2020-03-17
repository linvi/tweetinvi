using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace xUnitinvi.TestHelpers
{
    public static partial class AExtensions
    {
        public static AssertHttpRequest HttpRequest()
        {
            return new AssertHttpRequest();
        }

        public static AssertHttpRequest HttpRequest(AssertHttpRequestConfig config)
        {
            return new AssertHttpRequest(config);
        }

        public class AssertHttpRequest
        {
            private readonly Action<string> _log;
            private int _port;
            private TimeSpan _timeout;
            private Func<HttpListenerRequest, bool> _isExpectedRequest;

            public AssertHttpRequest()
            {
                _port = 8042;
                _timeout = TimeSpan.FromSeconds(30);
                _isExpectedRequest = request => true;
            }

            public AssertHttpRequest(AssertHttpRequestConfig config) : this()
            {
                _log = config?.Log;
            }

            public AssertHttpRequest OnPort(int port)
            {
                _port = port;
                return this;
            }

            public AssertHttpRequest WithATimeoutOf(TimeSpan timeout)
            {
                _timeout = timeout;
                return this;
            }

            public AssertHttpRequest Matching(Func<HttpListenerRequest, bool> isExpectedRequest)
            {
                _isExpectedRequest = isExpectedRequest;
                return this;
            }

            public async Task<HttpListenerRequest> MustHaveHappened()
            {
                using (var server = new HttpListener())
                {
                    server.Prefixes.Add($"http://*:{_port}/");
                    server.Start();

                    _log?.Invoke($"Server is now running at {_port}");

                    while (server.IsListening)
                    {
                        var contextTask = server.GetContextAsync();

                        if (await Task.WhenAny(contextTask, Task.Delay(_timeout)) == contextTask)
                        {
                            var context = await contextTask;
                            var request = context.Request;
                            var isValidRequest = _isExpectedRequest?.Invoke(request) ?? default;

                            context.Response.StatusCode = 200;
                            var matchedResponse = "SUCCESS => This request matches the validation criteria";
                            var notMatchedResponse = "NOT_MATCH => This request does not match the validation criteria";

                            var rawResponse = "HttpExpect => " + (isValidRequest ? matchedResponse : notMatchedResponse);
                            _log?.Invoke(rawResponse);
                            var responseContent = Encoding.ASCII.GetBytes(rawResponse);
                            await context.Response.OutputStream.WriteAsync(responseContent);
                            context.Response.Close();

                            if (isValidRequest)
                            {
                                return request;
                            }
                        }
                        else
                        {
                            throw new TimeoutException("HttpExpect server did not received http request matching criteria before Timeout");
                        }
                    }

                    throw new Exception("HttpExpect server stopped listening unexpectedly");
                }
            }
        }
    }
}