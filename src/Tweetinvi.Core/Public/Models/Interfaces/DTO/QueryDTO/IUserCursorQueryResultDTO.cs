namespace Tweetinvi.Models.DTO.QueryDTO
{
    public interface IUserCursorQueryResultDTO : IBaseCursorQueryDTO<IUserDTO>
    {
        IUserDTO[] Users { get; set; }
    }
}