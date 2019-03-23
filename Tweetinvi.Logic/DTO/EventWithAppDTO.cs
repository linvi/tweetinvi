using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Logic.DTO
{
    public class EventWithAppDTO : IEventWithAppDTO
    {
        public IEventDTO Event { get; set; }
        public IApp App { get; set; }
    }
}
