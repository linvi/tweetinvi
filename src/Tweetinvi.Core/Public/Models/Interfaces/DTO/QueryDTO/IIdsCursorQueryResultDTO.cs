namespace Tweetinvi.Models.DTO.QueryDTO
{
    public interface IIdsCursorQueryResultDTO : IBaseCursorQueryDTO<long>
    {
        long[] Ids { get; set; }
    }
}