using System;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BizHawk.Client.Common;
using BizHawk.Common;
using BizHawk.Emulation.Common;
using PokeAByte.BizHawk.StpTool.Domain;

namespace PokeAByte.BizHawk.StpTool.Application.Services;

public class MemoryMappedFilesService
{
    private readonly MemoryMappedFile PokeAByteMetadata_MemoryMappedFile;
    private readonly MemoryMappedViewAccessor PokeAByteMetadata_Accessor;

    private readonly MemoryMappedFile? PokeAByteData_MemoryMappedFile;
    private readonly MemoryMappedViewAccessor? PokeAByteData_Accessor;

    private readonly MemoryMappedFile? PokeAByteTimer_MemoryMappedFile;
    private readonly MemoryMappedViewAccessor? PokeAByteTimer_Accessor;
    
    public MemoryMappedFilesService(string metadataMemoryName, string dataMemoryName, string timerMemoryName)
    {
        PokeAByteMetadata_MemoryMappedFile = MemoryMappedFile.CreateOrOpen(metadataMemoryName, SharedPlatformConstants.BIZHAWK_METADATA_PACKET_SIZE, MemoryMappedFileAccess.ReadWrite);
        PokeAByteMetadata_Accessor = PokeAByteMetadata_MemoryMappedFile.CreateViewAccessor();

        PokeAByteData_MemoryMappedFile = MemoryMappedFile.CreateOrOpen(dataMemoryName, SharedPlatformConstants.BIZHAWK_DATA_PACKET_SIZE, MemoryMappedFileAccess.ReadWrite);
        PokeAByteData_Accessor = PokeAByteData_MemoryMappedFile.CreateViewAccessor();
        
        PokeAByteTimer_MemoryMappedFile = MemoryMappedFile.CreateOrOpen(timerMemoryName, sizeof(bool) + sizeof(double), MemoryMappedFileAccess.ReadWrite);
        PokeAByteTimer_Accessor = PokeAByteTimer_MemoryMappedFile.CreateViewAccessor();
    }

    public string WriteMetadata(
        ApiContainer? apiContainer,
        out SharedPlatformConstants.PlatformEntry? platformEntry,
        out int? frameSkip)
    {
        var data = new byte[SharedPlatformConstants.BIZHAWK_METADATA_PACKET_SIZE];

        data[0] = 0x00;
        data[1] = SharedPlatformConstants.BIZHAWK_INTEGRATION_VERSION;

        var system = apiContainer?.Emulation.GetGameInfo()?.System ?? string.Empty;
        Array.Copy(Encoding.UTF8.GetBytes(system), 0, data, 2, system.Length);

        PokeAByteMetadata_Accessor.WriteArray(0, data, 0, data.Length);

        platformEntry = SharedPlatformConstants.Information.SingleOrDefault(x => x.BizhawkIdentifier == system);

        if (string.IsNullOrWhiteSpace(system))
        {
            frameSkip = null;
            return "No game is loaded, doing nothing.";
        }

        if (platformEntry == null)
        {
            frameSkip = null;
            return $"{system} is not yet supported.";
        }

        frameSkip = platformEntry.FrameSkipDefault;
        return $"Sending {system} data to PokeAByte...";
    }

    public void WriteData(
        IMemoryDomains? memoryDomains,
        SharedPlatformConstants.PlatformEntry platformEntry,
        byte[] dataBuffer)
    {
        foreach (var entry in platformEntry.MemoryLayout)
        {
            try
            {
                var memoryDomain = memoryDomains?[entry.BizhawkIdentifier] ?? throw new Exception($"Memory domain not found.");
                
                memoryDomain.BulkPeekByte(0x00L.RangeToExclusive(entry.Length), dataBuffer);
                new Thread(() =>
                {
                    PokeAByteData_Accessor?.WriteArray(entry.CustomPacketTransmitPosition, dataBuffer, 0,
                        entry.Length);
                }).Start();
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to read memory domain {entry.BizhawkIdentifier}. {ex.Message}", ex);
            }
        }
    }

    public void WriteTimerData(double timer)
    {
        try
        {
            PokeAByteTimer_Accessor?.Write(sizeof(bool), timer);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception($"Unable to write timer data. {e.Message}");
        }
    }

    public bool GetTimerStarted()
    {
        try
        {
            return PokeAByteTimer_Accessor?.ReadBoolean(0) ?? false;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception($"Unable to read timer data. {e.Message}");
        }
    }
}