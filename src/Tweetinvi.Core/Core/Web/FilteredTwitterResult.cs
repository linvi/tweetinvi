namespace Tweetinvi.Core.Web
{
    public interface IFilteredTwitterResult<TDTO> : ITwitterResult<TDTO>
    {
        TDTO FilteredDTOs { get; set; }
    }

    public class FilteredTwitterResult<TDTO> : TwitterResult<TDTO>, IFilteredTwitterResult<TDTO>
    {
        public FilteredTwitterResult(ITwitterResult<TDTO> source)
        {
            Request = source.Request;
            Response = source.Response;
            DataTransferObject = source.DataTransferObject;
            FilteredDTOs = source.DataTransferObject;
        }

        public TDTO FilteredDTOs { get; set; }
    }
}