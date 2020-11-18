using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;

namespace HabBridge.Api.Serializer
{
    public class CookieContainerSerializer : JsonConverter
    {
        public override bool CanRead { get; } = true;
        public override bool CanWrite { get; } = true;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, (CookieContainer) value);

                writer.WriteValue(stream.ToArray());
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var value = (string) reader.Value;
            if (string.IsNullOrEmpty(value))
            {
                return new CookieContainer();
            }

            using (var stream = new MemoryStream(Convert.FromBase64String((string)reader.Value)))
            {
                var formatter = new BinaryFormatter();
                var container = (CookieContainer) formatter.Deserialize(stream);

                return container;
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(CookieContainer);
        }
    }
}
