using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace FerkopaUtils
{
    public static class XmlSerializerUtil
    {
        public static string Serialize<T>(T item)
        {
            var memStream = new MemoryStream();
            using (var textWriter = new XmlTextWriter(memStream, Encoding.Unicode))
            {
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(textWriter, item);
                memStream = textWriter.BaseStream as MemoryStream;
            }
            return memStream != null ? Encoding.Unicode.GetString(memStream.ToArray()) : null;
        }

        public static void Serialize<T>(T item,string filePath)
        {
           var serializer = new XmlSerializer(typeof (T));
           using (TextWriter textWriter = new StreamWriter(filePath,false,Encoding.UTF8))
           {

               serializer.Serialize(textWriter, item);
               textWriter.Close();
           }      
        }

        public static T Deserialize<T>(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException(nameof(filePath));
            if (File.Exists(filePath))
            {
                var deserializer = new XmlSerializer(typeof(T));
                using (var textReader = new StreamReader(filePath))
                {
                    var movies = (T) deserializer.Deserialize(textReader);
                    textReader.Close();
                    return movies;
                }
            }
            throw new FileNotFoundException(filePath);
        }

        public static string XmlSerializeToString(this object objectInstance)
        {
            var serializer = new XmlSerializer(objectInstance.GetType());
            var sb = new StringBuilder();

            using (TextWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, objectInstance);
            }

            return sb.ToString();
        }

        public static T XmlDeserializeFromString<T>(this string objectData)
        {
            return (T)XmlDeserializeFromString(objectData, typeof(T));
        }

        public static object XmlDeserializeFromString(this string objectData, Type type)
        {
            var serializer = new XmlSerializer(type);
            object result;

            using (TextReader reader = new StringReader(objectData))
            {
                result = serializer.Deserialize(reader);
            }

            return result;
        }

    }
}
