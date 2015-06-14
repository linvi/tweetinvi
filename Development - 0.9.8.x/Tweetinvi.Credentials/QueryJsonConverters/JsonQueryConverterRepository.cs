using System.Linq;
using Newtonsoft.Json;
using Tweetinvi.Core.Interfaces.DTO.QueryDTO;
using Tweetinvi.Credentials.QueryDTO;
using Tweetinvi.Logic.JsonConverters;

namespace Tweetinvi.Credentials.QueryJsonConverters
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
                new JsonInterfaceToObjectConverter<IUserCursorQueryResultDTO, UserCursorQueryResultDTO>(),
                new JsonInterfaceToObjectConverter<ITwitterListCursorQueryResultDTO, TwitterListCursorQueryResultDTO>(),
            });

            Converters = converters.ToArray();
        }
    }
}