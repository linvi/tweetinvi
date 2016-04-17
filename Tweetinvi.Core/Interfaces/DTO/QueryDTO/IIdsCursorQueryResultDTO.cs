namespace Tweetinvi.Core.Interfaces.DTO.QueryDTO
{
    public interface IIdsCursorQueryResultDTO : IBaseCursorQueryDTO<long>
    {
        long[] Ids { get; set; }
    }
}