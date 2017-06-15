using System;
using System.Linq;
using Tweetinvi;
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

            Auth.SetUserCredentials("CONSUMER_KEY", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_TOKEN_SECRET");
            var authenticatedUser = User.GetAuthenticatedUser().ToString();

            WelcomeText.Text = $"Welcome {authenticatedUser}";
            RefreshHomeTimeline();

            RefreshButton.TouchDown += (sender, args) =>
            {
                RefreshHomeTimeline();
            };
        }

        private void RefreshHomeTimeline()
        {
            var tweets = Timeline.GetHomeTimeline(20).ToArray();
            var tableSource = new TweetTimelineTableViewSource(tweets);

            TimelineTableView.Source = tableSource;
            TimelineTableView.ReloadData();
        }
    }
}