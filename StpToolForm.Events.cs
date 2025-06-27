using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using BizHawk.Client.Common;
using BizHawk.Client.EmuHawk;
using BizHawk.Common;
using BizHawk.Common.CollectionExtensions;
using BizHawk.Common.IOExtensions;
using BizHawk.Emulation.Common;
using PokeAByte.BizHawk.StpTool.Application.Helpers;
using PokeAByte.BizHawk.StpTool.Domain;

namespace PokeAByte.BizHawk.StpTool;

public partial class StpToolForm
{
    //========
    //Debugging
    //========
    private void saveFrameButton_Click(object sender, EventArgs e)
    {
        var save = GetSelectedSaveModel();
        if (save is null) return;
        File.WriteAllBytes("save.dat", save.StateData);
        //CreateXml();
        /*using var memStream = new MemoryStream();
        using var bw = new BinaryWriter(memStream);
        ((MainForm)MainForm).Emulator.AsStatable().SaveStateBinary(bw);
        bw.Flush();
        var data = memStream.ToArray();
        var compressedData = GZipHelpers.CompressData(data);
        File.WriteAllBytes("savestate.dat", compressedData);*/
    }
    private void loadFrameButton_Click(object sender, EventArgs e)
    {
        using var fs = new FileStream("savestate.dat", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        var bytes = fs.ReadAllBytes();
        var data = GZipHelpers.DecompressData(bytes);
        ((MainForm)MainForm).Emulator.AsStatable().LoadStateBinary(data);
    }

    //=========
    // Saves
    //=========
    private void renameSaveButton_Click(object sender, EventArgs e)
    {
        if (_saveStateService.CurrentSave is null) return;
        var editName = new EditNameDialog(_saveStateService.CurrentSave.FlagName);
        if (editName.ShowDialog() == DialogResult.OK)
        {
            _saveStateService.UpdateSave(_saveStateService.CurrentSave.Key,
                editName.NewName, 
                true);
            SaveSaveStatesFile();
            //SaveStatesToFile();
        }
    }
    private void deleteSaveButton_Click(object sender, EventArgs e)
    {
        if (_isPlayback || _isPaused || _isRecording) return;
        foreach (var save in GetSelectedSaveModels())
        {
            _saveStateService.DeleteSave(save);
        }

        SaveSaveStatesFile();
        //SaveStatesToFile();
    }
    //================
    // Browse
    //================
    private void replayMovieBrowse_Click(object sender, EventArgs e)
    {
        //clear saved list
        _saveStateService.Saves.Clear();
        flaggedSavesListBox.Items.Clear();
        var movieFolderPath = "";
        try
        {
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            string assemblyDirectory = Path.GetDirectoryName(assemblyLocation);
            if (!string.IsNullOrWhiteSpace(assemblyDirectory))
            {
                assemblyDirectory = assemblyDirectory.Substring(0, assemblyDirectory.LastIndexOf('\\'));
            }

            movieFolderPath = assemblyDirectory ?? "";
            Directory.CreateDirectory(movieFolderPath);
        }
        catch (Exception movieDirException)
        {
            if (movieDirException is IOException
                || movieDirException is UnauthorizedAccessException)
            {
                //TO DO : Pass error to user?
            }
            else throw;
        }

        var filterset = PokeAByteMainForm.MovieSession.Movie.GetFSFilterSet();
        var result = this.ShowFileSaveDialog(
            fileExt: $".{filterset.Filters[0].Extensions.First()}",
            filter: filterset,
            initDir: movieFolderPath,
            initFileName: $"{Game.Name}.bk2",
            muteOverwriteWarning: true);
        if (string.IsNullOrWhiteSpace(result)) return;
        replayFileTextBox.Text = result;
        var filename = result?.Substring(result.LastIndexOf('\\') + 1) ?? string.Empty;
        replayMovieLoadedLabel.Text = $"Loaded: {filename}";
        if (File.Exists($"{result}.pab"))
        {
            var saves = _saveStateService.LoadSavesFromFile(result + ".pab");
            _saveStateService.AddSaveRange(saves);
            savesListView.Update();
            foreach (var flag in _saveStateService.FlaggedSaves)
            {
                flaggedSavesListBox.Items.Add(flag);
            }
            flaggedSavesListBox.Update();
        }
    }
    //================
    // Record
    //================
    private void startRecordingButton_Click(object sender, EventArgs e)
    {
        if (_isRecording)
        {
            startRecordingButton.Text = "Start Record Movie";
            stopRecordingButton_Click(sender, e);
            return;
        }
        var result = MessageBox.Show(
            "Warning! This will clear your current replay file. " +
            "Would you like to proceed?"
            , "Confirmation", MessageBoxButtons.YesNo);
        if (result != DialogResult.Yes) return;
        flaggedSavesListBox.Items.Clear();
        _pokeAByteMovieService.StartNewMovie(PokeAByteMainForm,  replayFileTextBox.Text);
        _saveStateService.Saves.Clear();
        _isRecording = true;
        //_stopwatchTimer.Start();
        _saveStateTimer.Start();
        stateLabel.Text = "State: Recording Movie";
        startRecordingButton.Text = "Stop Record Movie";
    }
    private void playCollectButton_Click(object sender, EventArgs e)
    {
        if (_isRecording)
        {
            playCollectButton.Text = "Play/Collect";
            stopRecordingButton_Click(sender, e);
            return;   
        }
        var result = MessageBox.Show(
            "Warning! This will clear your current saves and re-collect them from your replay file. " +
            "Would you like to proceed?"
            , "Confirmation", MessageBoxButtons.YesNo);
        if (result != DialogResult.Yes) return;
        _saveStateService.Saves.Clear();
        _pokeAByteMovieService.PlayMovie(PokeAByteMainForm, replayFileTextBox.Text);
        playCollectButton.Text = "Stop Collect";
        stateLabel.Text = "State: Collecting Saves";
        _isRecording = true;
        //_stopwatchTimer.Start();
        _saveStateTimer.Start();
    }

    private void collectAfterSaveButton_Click(object sender, EventArgs e)
    {
        collectAfterSaveButton.Text = "Collect After Save";
        if (_isRecording)
        {
            stopRecordingButton_Click(sender, e);
            return;   
        }
        var result = MessageBox.Show(
            "Warning! This will clear your current saves and re-collect them from your replay file. " +
            "Would you like to proceed?"
            , "Confirmation", MessageBoxButtons.YesNo);
        
        if (result != DialogResult.Yes) return;

        var selectedItem = DeleteAfterSelectedSaveHelper();

        _pokeAByteMovieService.PlayMovie(PokeAByteMainForm, replayFileTextBox.Text);
        if (selectedItem is not null)
        {
            _saveStateService.LoadSaveState(PokeAByteMainForm, selectedItem.Key);
            currentKeyLabel.Text = "Current Key: " + selectedItem.Key;
        }
        collectAfterSaveButton.Text = "Stop Collect";
        stateLabel.Text = "State: Collecting Saves";
        
        _isRecording = true;
        _saveStateTimer.Start();
        //_stopwatchTimer.Start();
    }

    private void deleteAfterSaveButton_Click(object sender, EventArgs e)
    {
        if (_isPlayback || _isRecording) return;
        var result = MessageBox.Show(
            "Warning! This will clear your current saves after the selected point. " +
            "Would you like to proceed?"
            , "Confirmation", MessageBoxButtons.YesNo);
        
        if (result != DialogResult.Yes) return;
        DeleteAfterSelectedSaveHelper();
    }
    private SavesModel? DeleteAfterSelectedSaveHelper()
    {
        var selectedItem = GetSelectedSaveModel();
        if (_saveStateService.Saves.Count > 0 && selectedItem is not null)
        {
            var toRemove = _saveStateService
                .Saves
                .Where(x => x.Key > selectedItem.Key)
                .ToList();
            foreach (var item in toRemove)
            {
                _saveStateService.Saves.Remove(item);
            }
        }

        return selectedItem;
    }
    //================
    // Playback
    //================
    private void moviePlayButton_Click(object sender, EventArgs e)
    {
        if (_isRecording) return;
        if (_isPaused)
        {
            PokeAByteMainForm.UnpauseEmulator();
            _isPaused = false;
        }

        if (_isPlayback)
        {
            moviePlayButton.Text = "Play Movie";
            stopRecordingButton_Click(sender, e);
            return;
        }

        _isPlayback = true;
        _pokeAByteMovieService.PlayMovie(PokeAByteMainForm, replayFileTextBox.Text);
        moviePlayButton.Text = "Stop Movie";
        stateLabel.Text = "State: Playback";
        var selectedItem = GetSelectedSaveModel();
        if(selectedItem is null) return;
        _saveStateService.LoadSaveState(PokeAByteMainForm, selectedItem.Key);
        currentKeyLabel.Text = "Current Key: " + selectedItem.Key;
    }
    //===============
    // Stop
    //===============
    private void stopRecordingButton_Click(object sender, EventArgs e)
    {
        stateLabel.Text = "State: Saving...";
        ResetButtonText();
        _isPlayback = false;
        _saveStateTimer.Stop();
        //_stopwatchTimer.Stop();
        _pokeAByteMovieService.StopMovie(PokeAByteMainForm, _isRecording);
        flaggedSavesListBox.Items.Clear();
        foreach (var flag in _saveStateService.FlaggedSaves)
        {
            flaggedSavesListBox.Items.Add(flag);
        }
        flaggedSavesListBox.Update();
        if (!_isRecording)
        {
            stateLabel.Text = "State: Human Controlled";
            return;
        }
        try
        {
            SaveSaveStatesFile();
            _isRecording = false;
            stateLabel.Text = "State: Human Controlled";
        }
        catch (Exception exception)
        {
            Log.Error("Exception", $"{exception}");
        }
    }
    private void movieReplayPauseButton_Click(object sender, EventArgs e)
    {
        if (_isRecording) return;
        if (!_isPaused)
        {
            movieReplayPauseButton.Text = "Resume Emu";
            stateLabel.Text = "State: Paused";
            PokeAByteMainForm.PauseEmulator();
        }
        else
        {
            movieReplayPauseButton.Text = "Pause Emu";
            stateLabel.Text = "State: Human Controlled";
            PokeAByteMainForm.UnpauseEmulator();
        }

        _isPaused = !_isPaused;
    }
    private void saveStatesButton_Click(object sender, EventArgs e)
    {
        var originalLabelState = stateLabel.Text;
        stateLabel.Text = "State: Saving...";
        var filePath = replayFileTextBox.Text;
        var path = filePath.Substring(0, filePath.LastIndexOf('\\'));
        var fileName = filePath.Substring(filePath.LastIndexOf('\\')+1).Replace(".bk2", "");
        var result = this.ShowFileSaveDialog(
            fileExt: $".bk2",
            initDir: path,
            initFileName: $"{fileName}",
            muteOverwriteWarning: true);
        var original = replayFileTextBox.Text;
        Console.WriteLine($"{fileName}, {path}, {filePath}, {result}");
        if (!string.IsNullOrWhiteSpace(result))
        {
            replayFileTextBox.Text = result;
        }
        SaveSaveStatesFile();
        stateLabel.Text = originalLabelState;
        replayFileTextBox.Text = original;
    }
    //==========
    // Flags
    //==========
    private void browseFlagsFileButton_Click(object sender, EventArgs e)
    {
        var openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
        openFileDialog.Title = "Select Flags File";
        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            flagsFileTextBox.Text = openFileDialog.FileName;
            Log.Error("", $"Loading {openFileDialog.FileName}");
            if (string.IsNullOrWhiteSpace(flagsFileTextBox.Text)) return;
            //load the flags
            try
            {
                _flagsService.LoadXmlFile(flagsFileTextBox.Text);
                flagsFileListBox.Items.Clear();
                foreach (var flag in _flagsService.Flags)
                {
                    flagsFileListBox.Items.Add(flag);
                }
            }
            catch (Exception exception)
            {
                Log.Error("browseFlagsFileButton_Click", $"{exception}");
            }
        }
    }
    //========
    //Events
    //========
    private void saveSaveIntervalButton_Click(object sender, EventArgs e)
    {
        if(saveIntervalText.Value <= 0) return;
        _saveStateTimer.Stop();
        _saveStateTimerIntervalMs = (int)saveIntervalText.Value;
        _saveStateTimer.Interval = _saveStateTimerIntervalMs;
        _saveStateTimer.Start();
    }
    private void ReadFromClient(MemoryContract<byte[]>? clientData)
    {
        if (clientData?.Data is null || string.IsNullOrWhiteSpace(clientData.BizHawkIdentifier))
            return;
        var memoryDomain = MemoryDomains?[clientData.BizHawkIdentifier] ?? 
                           throw new Exception($"Memory domain not found.");
        memoryDomain.Enter();
        for (int i = 0; i < clientData.Data.Length; i++)
        {
            //0x244EC
            memoryDomain.PokeByte(clientData.MemoryAddressStart + i, clientData.Data[i]);
        }
        memoryDomain.Exit();
        //save state 
        if (_isRecording)
        {
            //var eventFlag = string.IsNullOrEmpty(clientData.Path) ? "" : $" - {clientData.EventFlag}";
            var flag = _flagsService
                .Flags
                .FirstOrDefault(x => x.Path.Equals(clientData.Path, StringComparison.InvariantCultureIgnoreCase));
            var flagName = $" - Path: {clientData.Path}";
            if (flag is not null)
            {
                flagName += $" - Description: {flag.EventDescription}";
            }

            if (!string.IsNullOrWhiteSpace(clientData.ValueString))
            {
                flagName += $" - Value: {clientData.ValueString}";
            }
            _shouldSaveState = true;
            _saveName = $"Poke-A-Byte Update{flagName}";
            //SaveState(true,$"Poke-A-Byte Update {flagName}");
        }
    }
    private void SaveStateService_ListChanged(object sender, ListChangedEventArgs e)
    {
        savesListView.VirtualListSize = _saveStateService.Saves.Count;
        if (e.ListChangedType == ListChangedType.ItemAdded)
        {
            var newIndex = e.NewIndex;
            savesListView.EnsureVisible(newIndex);
        }
    }
    private void OnSaveStateTimerElapsedHandler(object sender, ElapsedEventArgs elapsedEventArgs)
    {
        if (!ignoreTimerCheckbox.Checked)
            _shouldSaveState = true;
    }
    private void OnFileSaveTimerElapsedHandler(object sender, ElapsedEventArgs e)
    {
        if (!_isRecording) return;
        SaveSaveStatesFile();
    }

    private void SaveSaveStatesFile()
    {
        var filePath = replayFileTextBox.Text;
        if (string.IsNullOrWhiteSpace(filePath))
        {
            Log.Error("SaveSaveStatesFile", "Unable to read filepath");
            return;
        }
        var fileName = filePath.Substring(filePath.LastIndexOf('\\'));
        filePath = filePath.Substring(0, filePath.LastIndexOf('\\')) + fileName + ".pab";
        Log.Error("SaveSaveStatesFile", $"Saving {filePath}...");
        _saveStateService.SaveStatesToFile(filePath);
    }
    /*private void ReadFlaggedData(MemoryContract<byte[]>? memoryContract)
    {
        if (memoryContract is null || memoryContract.IsFlagged is false)
            return;
        //save current frame
        SaveState(memoryContract.IsFlagged, 
            string.IsNullOrWhiteSpace(memoryContract.FlagName) ? 
                "Auto Flagged" : 
                memoryContract.FlagName);
        
    }*/
    private void CreateXml()
    {
        var flags = new List<FlagModel>
        {
            new FlagModel
            {
                Path = "player.name",
                EventDescription = "Player Name Updated!"
            },
            new FlagModel
            {
                Path = "player.party.0.nickname",
                EventDescription = "Party 0 Nickname Updated!"
            }
        };
        var eventFlags = new FlagModels
        {
            Flags = flags.ToArray()
        };
        var bytes = SerializationHelpers.Serialize(eventFlags);
        File.WriteAllBytes("test.xml", bytes);
    }
    private void DrawDebugLabels()
    {
        currentFrameLabel.Text = $"Current Frame: {PokeAByteMainForm.Emulator.Frame}";
        currentSystemLabel.Text = $"Current System: {PokeAByteMainForm.Game.System}";
        currentGameLabel.Text = $"Current Game: {PokeAByteMainForm.Game.Name}";
        mousePositionLabel.Text = $"Mouse Position: ({timeScrubber1.ControlMousePosition.X},{timeScrubber1.ControlMousePosition.Y})";
        var ts = TimeSpan.FromSeconds(_currentFrameTime);
        stopwatchTimeLabel.Text = $"Current Time: {ts.Hours}:{ts.Minutes}:{ts.Seconds}.{ts.Milliseconds}";
    }
    private void MemoryMappedFileUpdate()
    {
        if (Platform == null) { return; }

        if (Platform.FrameSkipDefault != null)
        {
            FrameSkip -= 1;

            if (FrameSkip != 0) { return; }
        }
        //PokeAByteMainForm.PauseEmulator();
        _mmfService.WriteData(MemoryDomains, Platform, DataBuffer);
        //PokeAByteMainForm.UnpauseEmulator();
        /*new Thread(() =>
        {
            _mmfService.WriteData(MemoryDomains, Platform, DataBuffer);
        }).Start();*/

        if (FrameSkip == 0)
        {
            FrameSkip = Platform.FrameSkipDefault;
        }
    }
    private void ScrubMovie()
    {
        if(_isRecording) return;
        if (_isPlayback && !timeScrubber1.IsMouseDown)
        {
            //Get current movie frame
            var frame = PokeAByteMainForm.Emulator.Frame;
            //get index for frame
            var frameState = _saveStateService
                .Saves
                .FirstOrDefault(x => x.Frame == frame);
            if (frameState is not null)
            {
                UpdateScrubberPosition(frameState);
            }
        }
        if (!timeScrubber1.IsMouseDown) return;
        if (_saveStateService.Saves.Count == 0) return;
        var index = timeScrubber1.GetIndex(_saveStateService.Saves.Count);
        if(index >= _saveStateService.Saves.Count) 
            return;
        var found = _saveStateService.Saves[index];
        if (found is null) return;
        //savesListView.Items[found.Key].Selected = true;
        savesListView.EnsureVisible(found.Key);
        _saveStateService.LoadSaveState(PokeAByteMainForm, found.Key);
        currentKeyLabel.Text = "Current Key: " + found.Key;
        if (found.SaveTime == 0)
            _currentFrameTime = GetCurrentTime(found.Frame);
        else
            _currentFrameTime = (double)found.SaveTime / 1000;
    }

    private void StpToolForm_Resize(object sender, EventArgs e)
    {
        timeScrubber1.Invalidate();
    }

    private void SavesListView_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
    {
        if (e.ItemIndex >= 0 && e.ItemIndex < _saveStateService.Saves.Count)
        {
            var save = _saveStateService.Saves[e.ItemIndex];
            var item = new ListViewItem($"{save.Key}");
            item.SubItems.Add($"{save.Frame}");
            item.SubItems.Add($"{save.SaveTime}");
            item.SubItems.Add(save.FlagName);
            e.Item = item;
        }
        else
        {
            e.Item = new ListViewItem(string.Empty) {SubItems = {string.Empty, string.Empty, string.Empty}};
        }
    }
    private void SavesListView_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_isRecording) return;
        var selectedItem = GetSelectedSaveModel();
        if(selectedItem is null) return;
        _saveStateService.LoadSaveState(PokeAByteMainForm, selectedItem.Key);
        currentKeyLabel.Text = "Current Key: " + selectedItem.Key;
        if(selectedItem.SaveTime == 0)
            _currentFrameTime = GetCurrentTime(selectedItem.Frame);
        else
            _currentFrameTime = (double) selectedItem.SaveTime / 1000;
        if (timeScrubber1.IsMouseDown) return;
        UpdateScrubberPosition(selectedItem);
    }
    //============
    // Helpers
    //============
    private void UpdateScrubberPosition(SavesModel frameState)
    {
        var currentFrame = frameState.Key;
        //savesListView.Items[currentFrame].Selected = true;
        savesListView.EnsureVisible(currentFrame);
        timeScrubber1.UpdatePosition(currentFrame, _saveStateService.Saves.Count);
    }
    private void ReloadState()
    {
        if (_isPaused || _isRecording || !_isPlayback) return;
        var currentFrame = PokeAByteMainForm.Emulator.Frame;
        //find the frame
        var save = _saveStateService
            .Saves
            .FirstOrDefault(x => x.Frame == currentFrame /*&&
                                 !string.IsNullOrWhiteSpace(x.FlagName)*/);
        if (save is null) return;
        _saveStateService.LoadSaveState(PokeAByteMainForm, save.Key);
        //_currentFrameTime += (double)save.SaveTime / 1000;
        currentKeyLabel.Text = "Current Key: " + save.Key;
        if (save.Key + 1 < _saveStateService.Saves.Count)
        {
            _numFramesBeforeNextSave = _saveStateService.Saves[save.Key + 1].Frame - save.Frame;
            _timeBetweenNextSave = _saveStateService.Saves[save.Key+1].SaveTime - save.SaveTime;
        }
        //var currentState = _saveStateService.GetStateData(PokeAByteMainForm);
        //if (save.StateData.AsSpan().SequenceEqual(currentState)) return;
    }
    private void SaveStatesToFile()
    {
        var saveFile = replayFileTextBox.Text;
        if (string.IsNullOrWhiteSpace(saveFile)) return;
        _saveStateService.SaveStatesToFile($"{saveFile}.pab");
    }
    private void SaveState(bool isFlagged = false, string frameName = "")
    {
        if (!_isRecording || !_shouldSaveState) return;
        _saveStateTimer.Stop();
        _saveStateService.SaveState(PokeAByteMainForm, 
            _stopwatchTimer.ElapsedMilliseconds, 
            isFlagged, 
            frameName, 
            _startFrame);
        _saveStateTimer.Start();
        _shouldSaveState = false;
    }
    private SavesModel? GetSelectedSaveModel()
    {
        //var selectedItems = savesListView.SelectedItems.Cast<ListViewItem>().FirstOrDefault();
        var selectedIndex = savesListView.SelectedIndices.Cast<int>().FirstOrDefault();
        if (selectedIndex >= 0 && selectedIndex < _saveStateService.Saves.Count)
        {
            return _saveStateService.Saves[selectedIndex];
        }
        return null;
    }

    private List<SavesModel> GetSelectedSaveModels()
    {
        var selectedSaves = new List<SavesModel>();
        foreach (int index in savesListView.SelectedIndices)
        {
            if (index >= 0 && index < _saveStateService.Saves.Count)
            {
                selectedSaves.Add(_saveStateService.Saves[index]);
            }
        }
        return selectedSaves;
    }
    private void ResetButtonText()
    {
        moviePlayButton.Text = "Play Movie";
        playCollectButton.Text = "Play/Collect";
        startRecordingButton.Text = "Start Record Movie";
    }

    private double GetVSync()
    {
        var vidProvider = PokeAByteMainForm.Emulator.AsVideoProviderOrDefault();
        if (vidProvider is not null)
        {
            if(vidProvider.VsyncDenominator == 0) return 0.0;
            return (double)vidProvider.VsyncNumerator / vidProvider.VsyncDenominator;
        }
        if (PokeAByteMainForm.Emulator.VsyncDenominator() == 0) return 0.0;
        return (double)PokeAByteMainForm.Emulator.VsyncNumerator() / PokeAByteMainForm.Emulator.VsyncDenominator();
    }

    private double GetCurrentTime(int currentFrame = 0)
    {
        if(currentFrame == 0)
            currentFrame = PokeAByteMainForm.Emulator.Frame;
        double average = 0;
        var before = _saveStateService.Saves.LastOrDefault(x => x.Frame <= currentFrame);
        var after = _saveStateService.Saves.FirstOrDefault(x => x.Frame > currentFrame);
        if (before is null && after is not null)
        {
            if(after.SaveTime != 0)
                average = after.Frame / ((double)after.SaveTime/1000);
        }
        else if (before is not null && after is null)
        {
            if(before.SaveTime != 0)
                average = before.Frame / ((double)before.SaveTime/1000);
        }
        else if (before is not null && after is not null)
        {
            double avgBefore = 0, avgAfter = 0;
            if(before.SaveTime != 0)
                avgBefore = before.Frame / ((double)before.SaveTime/1000);
            if(after.SaveTime != 0)
                avgAfter = after.Frame / ((double)after.SaveTime/1000);
            average = (avgAfter + avgBefore) / 2.0;
        }
        if (average > 0)
            return currentFrame / average;
        return 0;
    }
    private void SetCurrentTime()
    {
        if(!_hasTimerStarted) return;
        _currentFrameTime = GetCurrentTime();
    }
    private void flaggedSavesListBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (flaggedSavesListBox.SelectedItem is not SavesModel selectedItem)
            return;
        _saveStateService.LoadSaveState(PokeAByteMainForm, selectedItem.Key);
        savesListView.EnsureVisible(selectedItem.Key);
    }
}