using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Tweetinvi;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Examplinvi.UniversalApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
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
                Message.Text = string.Format("Hi '{0}'. Welcome on board with Windows 10 Universal App!", User.GetLoggedUser().Name);
            }

            RunSampleStream();
        }

        private void RunSampleStream()
        {
            var uiDispatcher = Dispatcher;
            var s = Tweetinvi.Stream.CreateSampleStream();

            s.TweetReceived += (o, args) =>
            {
                uiDispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    Message.Text = args.Tweet.ToString();
                });
            };

            s.StartStreamAsync();
        }

    }
}