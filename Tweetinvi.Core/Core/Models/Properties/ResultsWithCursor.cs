using System.Collections.Generic;
using Tweetinvi.Models;

namespace Tweetinvi.Logic.Model
{
    public class ResultsWithCursor<T> : IResultsWithCursor<T>
    {
        public IEnumerable<T> Results { get; set; }
        public string Cursor { get; set; }
    }
}
