namespace Tweetinvi.Core.Interfaces.DTO.QueryDTO
{
    public interface IUserCursorQueryResultDTO : IBaseCursorQueryDTO<IUserDTO>
    {
        IUserDTO[] Users { get; set; }
    }
}