using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace xUnitinvi.TestHelpers
{
    public class NgrokRunner : IDisposable
    {
        private Process _process;
        private string _url;

        public void Start(int port)
        {
            _process = new Process
            {
                StartInfo =
                {
                    FileName = "ngrok",
                    Arguments = $"http {port}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    CreateNoWindow = true
                }
            };

            _process.Start();
        }

        public async Task<string> GetUrl()
        {
            if (_url == null)
            {
                await Task.Delay(2000); // delay for ngrok to initialize connection

                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync("http://localhost:4040/api/tunnels").ConfigureAwait(false);
                    var body = await response.Content.ReadAsStringAsync();
                    var match = Regex.Match(body, "\"public_url\":\"(?<url>https[^\"]*)");
                    _url = match.Groups["url"].Value;
                }
            }

            return _url;
        }

        public void Dispose()
        {
            _process?.StandardInput.Close();
            _process?.Kill();
            _process?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}