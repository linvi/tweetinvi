namespace Tweetinvi.Core.Web
{
    public interface IFilteredTwitterResult<TDTO> : ITwitterResult<TDTO>
    {
        TDTO FilteredDTO { get; set; }
    }

    public interface IFilteredTwitterResult<TDTO, TFilterResultDTO> : ITwitterResult<TDTO>
    {
        TFilterResultDTO FilteredResultsDTO { get; set; }
    }

    public class FilteredTwitterResult<TDTO> : TwitterResult<TDTO>, IFilteredTwitterResult<TDTO>
    {
        public FilteredTwitterResult(ITwitterResult<TDTO> source)
        {
            Request = source.Request;
            Response = source.Response;
            Model = source.Model;
            FilteredDTO = source.Model;
        }

        public TDTO FilteredDTO { get; set; }
    }

    public class FilteredTwitterResult<TDTO, TFilteredResultDTO> : TwitterResult<TDTO>, IFilteredTwitterResult<TDTO, TFilteredResultDTO>
    {
        public FilteredTwitterResult(ITwitterResult<TDTO> source, TFilteredResultDTO filteredResultsResultDTO)
        {
            Request = source.Request;
            Response = source.Response;
            Model = source.Model;
            FilteredResultsDTO = filteredResultsResultDTO;
        }

        public TFilteredResultDTO FilteredResultsDTO { get; set; }
    }
}