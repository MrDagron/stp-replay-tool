using System;

namespace PokeAByte.BizHawk.StpTool.Application.Services;

public class NamedPipeService : IDisposable
{
    private NamedPipeServer? _namedPipeServer = null;

    public void CreateNamedPipeServer(string pipeName, ClientDataHandler? handler)
    {
        _namedPipeServer = new NamedPipeServer();
        _namedPipeServer.ClientDataHandler += handler;
        _namedPipeServer.StartServer(pipeName);
    }

    public void Dispose()
    {
        _namedPipeServer?.Dispose();
        _namedPipeServer = null;
    }
}