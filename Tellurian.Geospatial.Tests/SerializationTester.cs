using System.IO;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tellurian.Geospatial.Tests
{
    static class SerializationTester<T>
    {
        public static T? XmlSerializeAndDeserialize(T value)
        {
            using var m = new MemoryStream();
            var s = new DataContractSerializer(typeof(T));
            s.WriteObject(m, value);
            m.Flush();
            m.Position = 0;
            return (T)s.ReadObject(m);
        }

        public static T? JsonSerializeAndDeserialize(T value)
        {
            var json = JsonSerializer.Serialize(value, Options);
            return JsonSerializer.Deserialize<T>(json, Options);
        }

        private static JsonSerializerOptions Options =>
            new JsonSerializerOptions()
            {
                // Good that serialization and deserialization does not require special options.
            };
    }
}

