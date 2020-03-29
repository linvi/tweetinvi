using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;

namespace Examplinvi.Android
{
    public class ViewHolder : Java.Lang.Object
    {
        public TextView TweetTxt { get; set; }
    }
    
    public class TimelineAdapter : BaseAdapter, IListAdapter
    {
        private Activity Activity;
        private List<TweetItem> Tweets;

        public TimelineAdapter(Activity activity, List<TweetItem> items)
        {
            Activity = activity;
            Tweets = items;
        }

        public override int Count => Tweets.Count;

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            // ReSharper disable once PossibleInvalidOperationException - we ensure that when we added a tweet the id was specified
            return (long)Tweets[position].Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? Activity.LayoutInflater.Inflate(Resource.Layout.ListViewDataTemplate, parent, false);
            var tweetTextView = view.FindViewById<TextView>(Resource.Id.TweetTextView);

            tweetTextView.Text = Tweets[position].Text;

            var createdByTextView = view.FindViewById<TextView>(Resource.Id.CreatedByTextView);

            createdByTextView.Text = Tweets[position].CreatedBy;

            return view;
        }

        public void Add(TweetItem item)
        {
            Tweets.Add(item);
        }

        public void Clear()
        {
            Tweets.Clear();
        }
    }
}