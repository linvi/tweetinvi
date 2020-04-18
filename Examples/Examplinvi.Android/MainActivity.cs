using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Examplinvi.Android
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            
            var creds = new TwitterCredentials("CONSUMER_KEY", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_TOKEN_SECRET");
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

                var tweets = tweetIterators.NextPage().Result;
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