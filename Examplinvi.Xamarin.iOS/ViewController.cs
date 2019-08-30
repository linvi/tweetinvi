using System;
using System.Linq;
using Tweetinvi;
using Tweetinvi.Models;
using UIKit;

namespace Examplinvi.Xamarin.iOS
{
    public partial class ViewController : UIViewController
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            var creds = new TwitterCredentials("CONSUMER_KEY", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_TOKEN_SECRET");
            Auth.SetCredentials(creds);

            var client = new TwitterClient(creds);

            var authenticatedUser = client.Users.GetAuthenticatedUser().Result.ToString();

            WelcomeText.Text = $"Welcome {authenticatedUser}";
            RefreshHomeTimeline();

            RefreshButton.TouchDown += (sender, args) =>
            {
                RefreshHomeTimeline();
            };
        }

        private void RefreshHomeTimeline()
        {
            var tweets = Timeline.GetHomeTimeline(20).Result.ToArray();
            var tableSource = new TweetTimelineTableViewSource(tweets);

            TimelineTableView.Source = tableSource;
            TimelineTableView.ReloadData();
        }
    }
}