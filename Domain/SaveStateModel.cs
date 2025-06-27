using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;

namespace PokeAByte.BizHawk.StpTool.Domain;

public class SaveStateModels
{
    private const int _chunkSize = 1024;
    public byte[] OriginalSaveState { get; set; }
    public List<SaveStateModel> SaveStates { get; set; }
    private SaveStateModel? _currentSaveState;

    public void ReconstructSaveStates()
    {
        var lastStateData = OriginalSaveState;
        
        foreach (var saveState in SaveStates)
        {
            var reconstructedState = new byte[saveState.ReconstructedSize];
            Array.Copy(lastStateData, reconstructedState, lastStateData.Length);
            foreach (var state in saveState.SaveStateDifference)
            {
                Array.Copy(lastStateData, 
                    state.Key, 
                    reconstructedState, 
                    state.Key, 
                    state.Value.Length);
            }
            saveState.ReconstructedSaveState = reconstructedState;
            lastStateData = reconstructedState;
        }
    }
    
    public void SaveState(
        byte[] stateData,
        int frame,
        long saveTimeMs,
        bool isFlagged = false, 
        string flagName = "",
        int startFrame = 0)
    {
        if (SaveStates.Count == 0)
        {
            OriginalSaveState = stateData;
        }
        
        var key = _currentSaveState is not null ? _currentSaveState.Key + 1 : 0;

        var prevReconstructedSaveState = _currentSaveState is null ? 
            OriginalSaveState :
            _currentSaveState.ReconstructedSaveState;
        var stateDifference = CompareStates(prevReconstructedSaveState, stateData);
        var newState = new SaveStateModel
        {
            Key = key,
            Frame = frame,
            IsFlagged = isFlagged,
            FlagName = flagName,
            SaveStateDifference = stateDifference,
            ReconstructedSaveState = stateData,
            SaveTime = saveTimeMs,
            FrameStartOffset = startFrame,
            ReconstructedSize = stateData.Length,
        };
        
        SaveStates.Add(newState);
        _currentSaveState = newState;
    }

    private static Dictionary<int, byte[]> CompareStates(byte[] originalState, byte[] newState)
    {
        var stateDifference = new Dictionary<int, byte[]>();
        var totalChunks = (int)Math.Ceiling((double)newState.Length / _chunkSize);
        for (var i = 0; i < totalChunks; i++)
        {
            var start = i * _chunkSize;
            var chunk = originalState
                .Skip(start)
                .Take(_chunkSize)
                .ToArray();
            var newChunk = newState
                .Skip(start)
                .Take(_chunkSize)
                .ToArray();
            var equal = chunk.SequenceEqual(newChunk);
            if (!equal)
            {
                stateDifference.Add(start, newChunk);
            }
        }
        return stateDifference;
    }
}
public class SaveStateModel : IComparable<SaveStateModel>
{    
    public int Key { get; set; }
    public int Frame { get; set; }
    public bool IsFlagged { get; set; } = false;
    public string FlagName { get; set; } = "";
    public long SaveTime { get; set; } = 0;
    public int FrameStartOffset { get; set; } = 0;
    public Dictionary<int, byte[]> SaveStateDifference { get; set; } = new();
    public int ReconstructedSize { get; set; }

    [JsonIgnore]
    public byte[] ReconstructedSaveState { get; set; } = [];
    private string DisplayName => !string.IsNullOrWhiteSpace(FlagName) ? $"#{Key} - Frame #{Frame} - {FlagName}" : $"#{Key} - Frame #{Frame}";
    public int CompareTo(SaveStateModel? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        return other is null ? 1 : Key.CompareTo(other.Key);
    }

    public override string ToString()
    {
        return DisplayName;
    }
}