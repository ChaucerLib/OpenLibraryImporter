using System.IO;
using System.IO.Compression;
using Newtonsoft.Json;

namespace Chaucer.Common
{
    public static class Compression
    {
        public static byte[] ToJsonSerializedGzipBytes<T>(T value, JsonSerializer jsonSerializer = null)
        {
            var serializer = jsonSerializer ?? new JsonSerializer();

            using (var memoryStream = new MemoryStream())
            {
                using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Compress, leaveOpen: true))
                using (var textWriter = new StreamWriter(gzipStream))
                using (var jsonWriter = new JsonTextWriter(textWriter))
                {
                    serializer.Serialize(jsonWriter, value);
                }
                return memoryStream.ToArray();
            }
        }

        public static T FromJsonSerializedGzipBytes<T>(byte[] jsonSerializedGzipBytes, JsonSerializer jsonSerializer = null)
        {
            var serializer = jsonSerializer ?? new JsonSerializer();
            using (var memoryStream = new MemoryStream(jsonSerializedGzipBytes))
            using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
            using (var textReader = new StreamReader(gzipStream))
            using (var jsonReader = new JsonTextReader(textReader))
            {
                return serializer.Deserialize<T>(jsonReader);
            }
        }
    }
}