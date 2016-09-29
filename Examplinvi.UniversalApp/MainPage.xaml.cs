using System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Tweetinvi;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Examplinvi.UniversalApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            Auth.SetUserCredentials("CONSUMER_KEY", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_TOKEN_SECRET");


            if (Auth.Credentials == null ||
                string.IsNullOrEmpty(Auth.Credentials.ConsumerKey) ||
                string.IsNullOrEmpty(Auth.Credentials.ConsumerSecret) ||
                string.IsNullOrEmpty(Auth.Credentials.AccessToken) ||
                string.IsNullOrEmpty(Auth.Credentials.AccessTokenSecret) ||
                Auth.Credentials.AccessToken == "ACCESS_TOKEN")
            {
                Message.Text = "Please enter your credentials in the MainPage.xaml.cs file";
            }
            else
            {
                var user = User.GetAuthenticatedUser();
                Message.Text = string.Format("Hi '{0}'. Welcome on board with Windows 10 Universal App!", user.Name);

                //PublishTweet();
                RunSampleStream();
            }
        }

        private void PublishTweet()
        {
            Tweet.PublishTweet("Check out #tweetinvi, the best c# library!");
        }

        private string _buffer;

        private void RunSampleStream()
        {
            var uiDispatcher = Dispatcher;
            var s = Stream.CreateSampleStream();

            var i = 0;

            s.TweetReceived += async (o, args) =>
            {
                _buffer += $"{args.Tweet.Text}\r\n";

                ++i;

                if (i == 10) // Every 10 tweets so that it makes it easier to handle for raspberry pi.
                {
                    i = 0;

                    await uiDispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        Message.Text = _buffer;
                    });

                    _buffer = string.Empty;
                }
            };

            s.StartStreamAsync();
        }

    }
}