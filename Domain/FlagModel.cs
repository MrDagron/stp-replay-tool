using System.Xml.Serialization;

namespace PokeAByte.BizHawk.StpTool.Domain;

public record FlagModel
{
    public string Path { get; set; }
    public string EventDescription { get; set; }
    public override string ToString()
    {
        return $"Path: {Path}, Description: {EventDescription}";
    }
}

[XmlRoot("Flags")]
public class FlagModels
{
    [XmlElement("Flag")] public FlagModel[] Flags { get; set; }
}