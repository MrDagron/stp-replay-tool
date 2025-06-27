using System.Collections.Generic;
using System.IO;
using System.Linq;
using PokeAByte.BizHawk.StpTool.Application.Helpers;
using PokeAByte.BizHawk.StpTool.Domain;

namespace PokeAByte.BizHawk.StpTool.Application.Services;

public class FlagsService
{
    public List<FlagModel> Flags { get; set; } = [];

    public void LoadXmlFile(string filename)
    {
        if (string.IsNullOrWhiteSpace(filename))
            return;
        if(!File.Exists(filename))
            return;
        var xmlData = File.ReadAllBytes(filename);
        var parsedData = SerializationHelpers.Deserialize<FlagModels>(xmlData);
        if (parsedData is null) return;
        Flags = parsedData.Flags.ToList();
    }
}