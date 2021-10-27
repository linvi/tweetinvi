using System.Text;
using Tweetinvi.Parameters.V2;

namespace Tweetinvi.Core.QueryGenerators.V2
{
    public interface ITimelinesV2QueryGenerator
    {
        string GetTimelineQuery(IGetTimelinesV2Parameters parameters);
        string GetMentionTimelineQuery(IGetTimelinesV2Parameters parameters);
        void AddTimelineFieldsParameters(IGetTimelinesV2Parameters parameters, StringBuilder query);
    }
}
