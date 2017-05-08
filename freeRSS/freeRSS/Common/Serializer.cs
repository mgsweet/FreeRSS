using System.IO;
using System.Runtime.Serialization;

//  引用自https://github.com/longjj/Windows-appsample-rssreader/blob/master/RssReader/Common/Serializer.cs

namespace freeRSS.Common
{
    /// <summary>
    /// Provides basic serialization and deserialization functions.
    /// </summary>
    public static class Serializer
    {
        /// <summary>
        /// Serializes the specified object as a byte array.
        /// </summary>
        public static byte[] Serialize<T>(T obj)
        {
            MemoryStream stream = new MemoryStream();
            DataContractSerializer dcs = new DataContractSerializer(typeof(T));
            dcs.WriteObject(stream, obj);
            return stream.ToArray();
        }

        /// <summary>
        /// Deserializes the specified byte array as an instance of type T. 
        /// </summary>
        public static T Deserialize<T>(byte[] buffer)
        {
            MemoryStream stream = new MemoryStream(buffer);
            DataContractSerializer dcs = new DataContractSerializer(typeof(T));
            return (T)dcs.ReadObject(stream);
        }
    }
}
