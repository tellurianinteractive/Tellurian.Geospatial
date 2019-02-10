using System.IO;
using System.Runtime.Serialization;

namespace Tellurian.Geospatial.Tests
{
    static class SerializationTester<T> where T : struct
    {
        public static T SerializeAndDeserialize(T value)
        {
            using (var m = new MemoryStream())
            {
                var s = new DataContractSerializer(typeof(T));
                s.WriteObject(m, value);
                m.Flush();
                m.Position = 0;
                return (T)s.ReadObject(m);
            }
        }
    }
}
