using System.IO;
using System.Xml.Serialization;

namespace PokeAByte.BizHawk.StpTool.Domain;

public record MemoryContract<T>
{
    public long MemoryAddressStart { get; set; } = 0x0L;
    public int DataLength { get; set; } = 0;
    public string DataType => typeof(T).Name;
    public T? Data { get; set; }
    public string BizHawkIdentifier { get; set; } = "";
    //public string EventFlag { get; set; } = "";
    public string Path { get; set; } = "";
    public string ValueString { get; set; } = "";

    public byte[] Serialize()
    {
        var xmlSerializer = new XmlSerializer(typeof(MemoryContract<T>));
        using var memoryStream = new MemoryStream();
        xmlSerializer.Serialize(memoryStream, this);
        return memoryStream.ToArray();
    }

    public static MemoryContract<T>? Deserialize(byte[] data)
    {
        var xmlSerializer = new XmlSerializer(typeof(MemoryContract<T>));
        var memoryStream = new MemoryStream(data);
        return (MemoryContract<T>?)xmlSerializer.Deserialize(memoryStream);
    }
}
