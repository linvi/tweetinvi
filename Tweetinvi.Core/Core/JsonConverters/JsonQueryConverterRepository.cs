using System.Linq;
using Newtonsoft.Json;
using Tweetinvi.Core.DTO.Cursor;
using Tweetinvi.Models.DTO.QueryDTO;

namespace Tweetinvi.Core.JsonConverters
{
    public static class JsonQueryConverterRepository
    {
        public static JsonConverter[] Converters { get; private set; }

        static JsonQueryConverterRepository()
        {
            Initialize();
        }

        private static void Initialize()
        {
            var converters = JsonPropertiesConverterRepository.Converters.ToList();
            converters.AddRange(new JsonConverter[]
            {
                new JsonInterfaceToObjectConverter<IIdsCursorQueryResultDTO, IdsCursorQueryResultDTO>(),
                new JsonInterfaceToObjectConverter<IMessageCursorQueryResultDTO, MessageCursorQueryResultDTO>(),
                new JsonInterfaceToObjectConverter<IUserCursorQueryResultDTO, UserCursorQueryResultDTO>(),
                new JsonInterfaceToObjectConverter<ITwitterListCursorQueryResultDTO, TwitterListCursorQueryResultDTO>(),
            });

            Converters = converters.ToArray();
        }
    }
}