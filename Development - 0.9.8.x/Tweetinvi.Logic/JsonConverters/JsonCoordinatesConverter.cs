using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Logic.DTO;

namespace Tweetinvi.Logic.JsonConverters
{
    public class JsonCoordinatesConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                return null;
            }

            var coordinatesArray = serializer.Deserialize<double[]>(reader);

            if (coordinatesArray == null)
            {
                return null;
            }

            var coordinates = new CoordinatesDTO(coordinatesArray[0], coordinatesArray[1]);

            if (objectType == typeof(List<ICoordinates>[]))
            {
                return new []
                {
                    new List<ICoordinates>
                    {
                        coordinates
                    }
                };
            }

            return coordinates;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            var isJsonConverterAvailable = objectType == typeof(ICoordinates) ||
                                           objectType == typeof(List<ICoordinates>[]);

            return isJsonConverterAvailable;
        }
    }
}