using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using BizHawk.Client.EmuHawk;
using BizHawk.Common;
using BizHawk.Emulation.Common;
using PokeAByte.BizHawk.StpTool.Application.Helpers;
using PokeAByte.BizHawk.StpTool.Domain;

namespace PokeAByte.BizHawk.StpTool.Application.Services;

public class SaveStateService
{
    public BindingList<SavesModel> Saves { get; set; } = [];
    public BindingList<SavesModel> FlaggedSaves { get; set; } = [];
    public SavesModel? CurrentSave { get; set; }
    private int _lastSize = 0;
    public void SaveState(MainForm mainForm,
        long saveTimeMs,
        bool isFlagged = false, 
        string flagName = "",
        int startFrame = 0)
    {
        var data = GetStateData(mainForm);
        var frame = mainForm.Emulator.Frame;
        if (data.Length == 0)
            return;
        _lastSize = data.Length;
        new Thread(() => {
            var compressedData = ZStdHelpers.Compress(data);
            var saveStateData = new SavesModel
            {
                Key = Saves.LastOrDefault() is not null ? Saves.Last().Key+1 : 0,
                Frame = frame,
                IsFlagged = isFlagged,
                FlagName = flagName,
                StateData = compressedData,
                SaveTime = saveTimeMs,
                FrameStartOffset = startFrame
            };
            Saves.Add(saveStateData);
            if (isFlagged)
            {
                FlaggedSaves.Add(saveStateData);
            }
        }).Start();
    }
    private byte[] GetStateData(MainForm mainForm)
    {
        try
        {
            using var memoryStream = new MemoryStream(_lastSize);
            using var bw = new BinaryWriter(memoryStream);
            mainForm.Emulator.AsStatable().SaveStateBinary(bw);
            bw.Flush();
            return memoryStream.ToArray();
        }
        catch (Exception e)
        {
            Log.Error("SaveState", e.Message);
            return [];
        }

    }
    public void LoadSaveState(MainForm mainForm, int key)
    {
        var data = Saves.FirstOrDefault(x => x.Key == key);
        if(data is null)
            return;
        var uncompressedData = ZStdHelpers.Decompress(data.StateData);
        mainForm.Emulator.AsStatable().LoadStateBinary(uncompressedData);
        CurrentSave = data;
    }
    private bool _isSaving = false;
    public void SaveStatesToFile(string path)
    {
        if (_isSaving)
        {
            Log.Error("SaveStatesToFile", "Already saving states.");
            return;
        }
        _isSaving = true;
        
        Log.Error("SaveStatesToFile", $"Saving states to {path}... State length: {Saves.Count}");

        try
        {
            var saves = Saves.ToList();
            SerializationHelpers.SerializeJsonToFile(saves, path);
        }
        catch (Exception e)
        {
            Log.Error("SaveStatesToFile", e.Message);
        }
        finally
        {
            _isSaving = false;
        }

    }
    public List<SavesModel> LoadSavesFromFile(string path)
    {
        try
        {
            var deserializedData = SerializationHelpers.DeserializeJsonFromFile<List<SavesModel>>(path);
            return deserializedData ?? [];
        }
        catch (Exception e)
        {
            Log.Error("SaveStatesToFile", e.Message);
            return [];
        }
    }

    public void AddSaveRange(List<SavesModel> saves)
    {
        foreach (var save in saves)
        {
            Saves.Add(save);
            if (save.IsFlagged)
            {
                FlaggedSaves.Add(save);
            }
        }
    }

    public void UpdateSave(int key, string flagName, bool isFlagged)
    {
        var foundSave = Saves.FirstOrDefault(x => x.Key == key);
        Log.Error("UpdateSave", $"Save: {foundSave?.Key}");
        if (foundSave is null) return;
        Saves.Remove(foundSave);
        foundSave.IsFlagged = isFlagged;
        foundSave.FlagName = flagName;
        var index = Saves.ToList().BinarySearch(foundSave);
        if (index < 0) index = ~index;
        Saves.Insert(index, foundSave);
        
        if (!foundSave.IsFlagged) return;
        FlaggedSaves.Remove(foundSave);
        var flagIndex = FlaggedSaves.ToList().BinarySearch(foundSave);
        if (flagIndex < 0) flagIndex = ~flagIndex;
        FlaggedSaves.Insert(flagIndex, foundSave);
    }

    public void DeleteSave(SavesModel save)
    {
        if (Saves.Contains(save))
        {
            Saves.Remove(save);
            if (save.IsFlagged)
            {
                FlaggedSaves.Remove(save);
            }
        }
    }

    public SavesModel? GetPreviousSave(int cf)
    {
        return cf == 0 ? null : Saves.FirstOrDefault(x => x.Frame == cf - 1);
    }
}