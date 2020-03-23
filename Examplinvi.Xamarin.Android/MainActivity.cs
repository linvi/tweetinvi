using Android.App;
using Android.OS;
using Android.Widget;
using Examplinvi.Xamarin.Android.Resources;
using System.Collections.Generic;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Examplinvi.Xamarin.Android
{
    [Activity(Label = "Examplinvi.Xamarin.Android", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            var creds = new TwitterCredentials("CONSUMER_KEY", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_TOKEN_SECRET");
            Auth.SetCredentials(creds);

            var client = new TwitterClient(creds);

            var authenticatedUser = client.Users.GetAuthenticatedUser().Result;

            TextView status = FindViewById<TextView>(Resource.Id.Status);
            status.Text = string.Format("Welcome {0}", authenticatedUser.ToString());

            var items = new List<TweetItem>();

            ListView timelineListView = FindViewById<ListView>(Resource.Id.Timeline);
            var timelineListAdapter = new TimelineAdapter(this, items);
            timelineListView.Adapter = timelineListAdapter;

            Button button = FindViewById<Button>(Resource.Id.TimelineButton);
            button.Click += delegate
            {
                var tweetIterators = client.Timelines.GetHomeTimelineIterator(new GetHomeTimelineParameters()
                {
                    PageSize = 20
                });

                timelineListAdapter.Clear();

                var tweets = tweetIterators.MoveToNextPage().Result;
                foreach (var tweet in tweets)
                {
                    timelineListAdapter.Add(new TweetItem
                    {
                        Id = tweet.Id,
                        Text = tweet.Text,
                        CreatedBy = tweet.CreatedBy.Name
                    });
                }

                timelineListAdapter.NotifyDataSetChanged();

                button.Text = "CLICK to Refresh Home Timeline";
            };
        }
    }
}

