namespace Tweetinvi.Core.Interfaces.DTO.QueryDTO
{
    public interface ITwitterListCursorQueryResultDTO : IBaseCursorQueryDTO<ITwitterListDTO>
    {
        ITwitterListDTO[] TwitterLists { get; set; }
    }
}
