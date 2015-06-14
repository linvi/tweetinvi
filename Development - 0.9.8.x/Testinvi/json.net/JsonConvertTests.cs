using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Testinvi.json.net
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class JsonConvertTests
    {
        public interface IMediaSize
        {
            int? Width { get; set; }
            int? Height { get; set; }
            string Resize { get; set; }

            Dictionary<string, IMediaSize2> MediaSizes2 { get; set; }
        }

        private class MediaSize : IMediaSize
        {
            [JsonProperty("w")]
            public int? Width { get; set; }

            [JsonProperty("h")]
            public int? Height { get; set; }

            [JsonProperty("resize")]
            public string Resize { get; set; }

            [JsonProperty("sizes2")]
            //[JsonConverter(typeof(JsonMediaSizeConverter))]
            public Dictionary<string, IMediaSize2> MediaSizes2 { get; set; }
        }

        public interface IMediaSize2
        {
            int? Width { get; set; }
            int? Height { get; set; }
            string Resize { get; set; }
        }

        private class MediaSize2 : IMediaSize2
        {
            [JsonProperty("w")]
            public int? Width { get; set; }

            [JsonProperty("h")]
            public int? Height { get; set; }

            [JsonProperty("resize")]
            public string Resize { get; set; }
        }

        private interface IMyLastMediaSize
        {
            string Hello { get; set; }
        }

        public class MyLastMediaSize : IMyLastMediaSize
        {
            [JsonProperty("hello")]
            public string Hello { get; set; }
        }

        private interface IMedia
        {
            IMyLastMediaSize Hello { get; set; }
            Dictionary<string, IMediaSize> MediaSizes { get; set; }
        }

        private class Media : IMedia
        {
            [JsonProperty("sizes")]
            //[JsonConverter(typeof(JsonMediaSizeConverter))]
            public Dictionary<string, IMediaSize> MediaSizes { get; set; }

            [JsonProperty("mediaSize")]
            [JsonConverter(typeof(JsonMediaSizeConverter<IMyLastMediaSize, MyLastMediaSize>))]
            public IMyLastMediaSize Hello { get; set; }
        }


        public class JsonMediaSizeConverter<T, U> : JsonConverter
        {
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                return serializer.Deserialize<U>(reader);
            }

            public override bool CanConvert(Type objectType)
            {
                var canConvert = objectType == typeof (T);
                return canConvert;
            }
        }



        [TestMethod]
        public void Deserialize_ToDictionaryOfStringToInterface()
        {
            // Arrange
            string mediaSize =
                "{" +
                "       \"mediaSize\":{\"hello\":\"1024\"}," +
                "\"sizes\":" +
                "   {   " +
                "       \"small\":{\"w\":340,\"h\":191,\"resize\":\"fit\"}," +
                "       \"thumb\":{\"w\":150,\"h\":150,\"resize\":\"crop\"}," +
                "       \"medium\":{\"w\":600,\"h\":337,\"resize\":\"fit\"}," +
                "       \"large\":{\"w\":1024,\"h\":576,\"resize\":\"fit\"}," +
                "       \"sizes2\":" +
                "       {   " +
                "           \"small\":{\"w\":340,\"h\":191,\"resize\":\"fit\"}," +
                "           \"thumb\":{\"w\":150,\"h\":150,\"resize\":\"crop\"}," +
                "           \"medium\":{\"w\":600,\"h\":337,\"resize\":\"fit\"}," +
                "           \"large\":{\"w\":1024,\"h\":576,\"resize\":\"fit\"}" +
                "       }" +
                "   }" +
                "}";

            // Act
            var jsonMediaConverter = new JsonMediaSizeConverter<IMedia, Media>();
            var jsonMediaSizeConverter = new JsonMediaSizeConverter<IMediaSize, MediaSize>();
            var jsonMediaSize2Converter = new JsonMediaSizeConverter<IMediaSize2, MediaSize2>();
            var result = JsonConvert.DeserializeObject<IMedia>(mediaSize, new JsonConverter[]
            {
                jsonMediaConverter,
                jsonMediaSizeConverter,
                jsonMediaSize2Converter
            });

            // Assert
            Assert.AreNotEqual(result, null);
        }
    }
}