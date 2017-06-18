using System;
using Foundation;
using Tweetinvi.Models;
using UIKit;

namespace Examplinvi.Xamarin.iOS
{
    public class TweetTimelineTableViewSource : UITableViewSource
    {
        private readonly ITweet[] _tweets;

        public TweetTimelineTableViewSource(ITweet[] tweets)
        {
            _tweets = tweets;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = new UITableViewCell(UITableViewCellStyle.Default, string.Empty);

            var tweetText = _tweets[indexPath.Row].Text;

            cell.TextLabel.Text = tweetText;

            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return _tweets.Length;
        }
    }
}
