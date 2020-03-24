namespace Tweetinvi.Models.DTO.QueryDTO
{
    public interface ITwitterListCursorQueryResultDTO : IBaseCursorQueryDTO<ITwitterListDTO>
    {
        ITwitterListDTO[] TwitterLists { get; set; }
    }
}
