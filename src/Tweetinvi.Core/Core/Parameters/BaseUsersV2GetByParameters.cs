using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Parameters
{
    public interface IBaseUsersV2GetByParameters : IBaseUsersV2Parameters
    {
        string By { get; }
    }

    public class BaseUsersV2GetByParameters : BaseUsersV2Parameters, IBaseUsersV2GetByParameters
    {
        public BaseUsersV2GetByParameters(string by)
        {
            By = by;
        }

        public string By { get; }
    }
}