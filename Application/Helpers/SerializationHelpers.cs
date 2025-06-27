using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PokeAByte.BizHawk.StpTool.Domain;

namespace PokeAByte.BizHawk.StpTool.Application.Helpers;

public static class SerializationHelpers
{
    public static byte[]? Serialize<T>(T classToSerialize)
    {
        if (classToSerialize is null)
            return null;
        try
        {
            var serializer = new XmlSerializer(typeof(T));
            using var sw = new StringWriter();
            serializer.Serialize(sw, classToSerialize);
            return Encoding.UTF8.GetBytes(sw.ToString());
        }
        catch (Exception)
        {
            return null;
        }
    }

    public static byte[]? SerializeJson<T>(T classToSerialize)
    {
        using var memoryStream = new MemoryStream();
        using var sw = new StreamWriter(memoryStream);
        using var jsonWriter = new JsonTextWriter(sw);
        try
        {
            var serializer = new JsonSerializer();
            serializer.Serialize(jsonWriter, classToSerialize);
            jsonWriter.Flush();
            return memoryStream.ToArray();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return default;
        }
    }

    public static void SerializeJsonToFile<T>(T classToSerialize, string filePath)
    {
        using var sw = new StreamWriter(filePath);
        using var jsonWriter = new JsonTextWriter(sw);
        var serializer = new JsonSerializer();
        serializer.Serialize(jsonWriter, classToSerialize);
        jsonWriter.Flush();
    }
    public static T? DeserializeJson<T>(byte[] bytes)
    {
        if (bytes.Length == 0) return default;
        var deserializer = new JsonSerializer();
        using var memoryStream = new MemoryStream(bytes);
        using var sr = new StreamReader(memoryStream);
        using var jsonReader = new JsonTextReader(sr);
        return deserializer.Deserialize<T>(jsonReader);
    }
    public static T? DeserializeJsonFromFile<T>(string filePath)
    {
        using var sr = new StreamReader(filePath);
        using var jsonReader = new JsonTextReader(sr);
        var deserializer = new JsonSerializer();
        return deserializer.Deserialize<T>(jsonReader);
    }
    /*public static SavesModelCollection? XmlStream(byte[] serializationData)
    {
        if (serializationData.Length == 0)
            return default;

        using var memoryStream = new MemoryStream(serializationData);
        using var sr = new StreamReader(memoryStream, Encoding.ASCII);
        var settings = new XmlReaderSettings
        {
            IgnoreComments = true,
            IgnoreProcessingInstructions = true,
        };
        using var reader = XmlReader.Create(sr, settings);
        try
        {
            while (reader.Read())
            {
                Console.WriteLine($"{reader.Name} - {reader.NodeType} - {reader.Value}");
            }
            return new SavesModelCollection();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return default;
        }
    }*/
    
public static T? Deserialize<T>(byte[] serializedObject)
    {
        if (serializedObject.Length == 0)
            return default;
        try
        {
            var serializer = new XmlSerializer(typeof(T));
            string xmlFromBytes = Encoding.UTF8.GetString(serializedObject);
            using var sr = new StringReader(xmlFromBytes);
            return (T)serializer.Deserialize(sr);
        }
        catch (Exception)
        {
            return default;
        }
    }
}