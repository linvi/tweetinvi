using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;

namespace Examplinvi.UAP
{
    public class StreamViewModel : INotifyPropertyChanged
    {
        private ITwitterClient _client;

        /// <summary>
        /// Informational message to the user.
        /// </summary>
        public string Message { get; private set; }

        private string streamingText;

        /// <summary>
        /// Tweets from the streaming API.
        /// </summary>
        public string StreamingText
        {
            get { return this.streamingText; }
            set
            {
                this.streamingText = value;
                NotifyPropertyChanged(nameof(this.StreamingText));
            }
        }

        public StreamViewModel()
        {
            Authenticate();
        }

        private ITwitterCredentials Credentials { get; set; } = new TwitterCredentials("CONSUMER_TOKEN", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_TOKEN_SECRET");
        private async Task Authenticate()
        {
            if (Credentials == null)
            {
                Message = "Please enter your credentials in the StreamViewModel.cs file";
            }
            else
            {
                _client = new TwitterClient(Credentials);

                var user = await _client.Users.GetAuthenticatedUser();
                Message = $"Hi '{user.Name}'. Welcome on board with Windows 10 Universal App!";                
            }
        }

        public async Task PublishTweet()
        {
            await _client.Tweets.PublishTweet("Check out #tweetinvi, the best c# library!");
        }

        private string _buffer;

        public void RunSampleStream()
        {
            var uiDispatcher = Windows.UI.Core.CoreWindow.GetForCurrentThread().Dispatcher;

            var s = _client.Streams.CreateSampleStream();
            var i = 0;

            s.TweetReceived += async (o, args) =>
            {
                _buffer += $"{args.Tweet.Text}\r\n";

                ++i;

                if (i == 10) // Every 10 tweets so that it makes it easier to handle for raspberry pi.
                {
                    i = 0;

                    await uiDispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        StreamingText = _buffer;
                    });

                    _buffer = string.Empty;
                }
            };

            s.StartStream();
        }

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
