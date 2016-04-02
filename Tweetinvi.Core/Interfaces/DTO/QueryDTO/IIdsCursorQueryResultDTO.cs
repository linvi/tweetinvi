namespace Tweetinvi.Core.Interfaces.DTO.QueryDTO
{
    public interface IIdsCursorQueryResultDTO : IBaseCursorQueryDTO<long>
    {
        long[] Ids { get; set; }
    }

    public interface IRetweetsCursorQueryResultDTO : IBaseCursorQueryDTO<ITweetDTO>
    {
        ITweetDTO[] results { get; set; }
    }
}