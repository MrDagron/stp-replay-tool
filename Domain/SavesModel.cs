using System;
using System.Xml.Serialization;

namespace PokeAByte.BizHawk.StpTool.Domain;

public class SavesModel : IComparable<SavesModel>
{
    public int Key { get; set; }
    public int Frame { get; set; }
    public bool IsFlagged { get; set; } = false;
    public byte[] StateData { get; set; } = [];
    public string FlagName { get; set; } = "";
    public long SaveTime { get; set; } = 0;
    public int FrameStartOffset { get; set; } = 0;
    public int GetCurrentFrameOffset()
    {
        if (Frame <= FrameStartOffset)
        {
            return 0;
        }

        return Frame - FrameStartOffset;
    }
    private string DisplayName => !string.IsNullOrWhiteSpace(FlagName) ? $"#{Key} - Frame #{Frame} - {FlagName}" : $"#{Key} - Frame #{Frame}";

    public int CompareTo(SavesModel? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        return other is null ? 1 : Key.CompareTo(other.Key);
    }

    public override string ToString()
    {
        return DisplayName;
    }
}
[XmlRoot("SavesModelCollection")]
public class SavesModelCollection
{
    [XmlElement("SavesModel")]
    public SavesModel[] SavesModels { get; set; }
}