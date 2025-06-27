using System.IO;
using System.IO.Compression;

namespace PokeAByte.BizHawk.StpTool.Application.Helpers;

public static class GZipHelpers
{
    public static byte[] CompressData(byte[] data)
    {
        using var compressedStream = new MemoryStream();
        using (var gzip = new GZipStream(compressedStream, CompressionMode.Compress))
        {
            gzip.Write(data, 0, data.Length);
        } 

        return compressedStream.ToArray();
    }

    public static byte[] DecompressData(byte[] data)
    {
        using var compressedStream = new MemoryStream(data);
        using var uncompressedStream = new MemoryStream();
        using var gzip = new GZipStream(compressedStream, CompressionMode.Decompress);
        gzip.CopyTo(uncompressedStream);
        return uncompressedStream.ToArray();
    }
}