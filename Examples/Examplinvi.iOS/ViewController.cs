using Foundation;
using System;
using Tweetinvi;
using Tweetinvi.Models;
using UIKit;

namespace Examplinvi.iOS
{
    public partial class ViewController : UIViewController
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        private ITwitterClient _client;
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            var creds = new TwitterCredentials("CONSUMER_KEY", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_TOKEN_SECRET");
            _client = new TwitterClient(creds);

            var authenticatedUser = _client.Users.GetAuthenticatedUser().Result.ToString();

            WelcomeText.Text = $"Welcome {authenticatedUser}";
            RefreshHomeTimeline();

            RefreshButton.TouchDown += (sender, args) =>
            {
                RefreshHomeTimeline();
            };
        }


        private void RefreshHomeTimeline()
        {
            var tweets = _client.Timelines.GetHomeTimeline().Result;
            var tableSource = new TweetTimelineTableViewSource(tweets);

            TimelineTableView.Source = tableSource;
            TimelineTableView.ReloadData();
        }
    }
}