using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BizHawk.Client.Common;
using BizHawk.Client.EmuHawk;
using BizHawk.Common.CollectionExtensions;
using BizHawk.Emulation.Common;
using PokeAByte.BizHawk.StpTool.Application.Services;
using PokeAByte.BizHawk.StpTool.Domain;
using MemoryMappedFilesService = PokeAByte.BizHawk.StpTool.Application.Services.MemoryMappedFilesService;
using NamedPipeService = PokeAByte.BizHawk.StpTool.Application.Services.NamedPipeService;
using SaveStateService = PokeAByte.BizHawk.StpTool.Application.Services.SaveStateService;
using Timer = System.Timers.Timer;

namespace PokeAByte.BizHawk.StpTool;

[ExternalTool("Poke-A-Byte STP Tool")]
public partial class StpToolForm : ToolFormBase, IExternalToolForm
{
    protected override string WindowTitleStatic // required when superclass is ToolFormBase
        => "Poke-A-Byte Integration Tool";
    
    public ApiContainer? APIs { get; set; }
    [RequiredService]
    public IMemoryDomains? MemoryDomains { get; set; }

    
    private MainForm PokeAByteMainForm => (MainForm)MainForm;

    //Services
    private readonly MemoryMappedFilesService _mmfService;
    private readonly NamedPipeService _pipeService;
    //private readonly NamedPipeService _flagsPipeService;
    private readonly SaveStateService _saveStateService;
    private readonly PokeAByteMovieService _pokeAByteMovieService;
    private readonly FlagsService _flagsService;
    
    //Props
    private string _system = string.Empty;
    private byte[] DataBuffer { get; } = new byte[SharedPlatformConstants.BIZHAWK_DATA_PACKET_SIZE];
    private SharedPlatformConstants.PlatformEntry? Platform = null;
    private int? FrameSkip = null;
    private readonly string _metadataMemoryName = "POKEABYTE_BIZHAWK.bin";
    private readonly string _dataMemoryName = "POKEABYTE_BIZHAWK_DATA.bin";
    private readonly string _timerMemoryName = "POKEABYTE_BIZHAWK_TIMER.bin";
    private bool _shouldSaveState = false;
    public string? _saveName = "";
    private bool _isRecording = false;
    private bool _isPlayback = false;
    private int _saveStateTimerIntervalMs = 1000;
    private bool _isPaused;
    private Timer _saveStateTimer;
    private Timer _fileSaveTimer;

    private bool _hasTimerStarted = false;
    private bool _timerStartedBackup = false;
    private double _currentFrameTime = 0.0;
    private bool _isRunOver = false;
    private int _startFrame = 0;
    private long _timeBetweenNextSave = 0;
    private int _numFramesBeforeNextSave = 0;
    private StpStopwatch _stopwatchTimer;
    private bool _hasMovieFinished = false;
    public StpToolForm()
    {
        _mmfService = new MemoryMappedFilesService(_metadataMemoryName, _dataMemoryName, _timerMemoryName);

        _pipeService = new NamedPipeService();
        _pipeService.CreateNamedPipeServer("BizHawk_Named_Pipe", ReadFromClient);
        /*new Thread(() =>
        {
            _pipeService.CreateNamedPipeServer("BizHawk_Named_Pipe", ReadFromClient);
        }).Start();*/
        _flagsService = new FlagsService();
        /*_flagsPipeService = new NamedPipeService();
        _flagsPipeService.CreateNamedPipeServer("BizHawk_MapperFlags_Named_Pipe", ReadFlaggedData, NamedPipeServerStream.MaxAllowedServerInstances);*/
        InitializeComponent();
        InitializeSavesListView();
        replayFileTextBox.Text = "";
        replayMovieLoadedLabel.Text = "";
        statusLabel.Text = "";
        flagsLoadedLabel.Text = "";
        flagFileTextBox.Text = "";
        _pokeAByteMovieService = new PokeAByteMovieService();
        _saveStateService = new SaveStateService();
        _saveStateService.Saves.ListChanged += SaveStateService_ListChanged;
        //savesListBox.DataSource = _saveStateService.Saves;
        _saveStateTimer = new Timer(_saveStateTimerIntervalMs);
        _saveStateTimer.AutoReset = false;
        _saveStateTimer.Elapsed += OnSaveStateTimerElapsedHandler;
        
        _fileSaveTimer = new Timer(1000*60);
        _fileSaveTimer.AutoReset = true;
        _fileSaveTimer.Elapsed += OnFileSaveTimerElapsedHandler;
        _fileSaveTimer.Enabled = true;
        Resize += StpToolForm_Resize;

        stateLabel.Text = "State: Human Controlled";
        _stopwatchTimer = new StpStopwatch();

        //flaggedSavesListBox.DataSource = _saveStateService.FlaggedSaves;
        /*_timerStopwatch.Start();*/
        //timeScrubber1.MouseMove += ScrubMovie;
    }

    private void InitializeSavesListView()
    {
        savesListView.RetrieveVirtualItem += SavesListView_RetrieveVirtualItem;
        savesListView.SelectedIndexChanged += SavesListView_SelectedIndexChanged;
        savesListView.Columns.Add("Key", 50);
        savesListView.Columns.Add("Frame", 75);
        savesListView.Columns.Add("Save Time", 75);
        savesListView.Columns.Add("Event", 300);
        savesListView.FullRowSelect = true;
    }
    public override void Restart()
    {
        statusLabel.Text = _mmfService.WriteMetadata(APIs, out Platform, out FrameSkip);
        base.Restart();
    }
    protected override void UpdateAfter()
    {
        if (_isRecording)
            _currentFrameTime = (double)_stopwatchTimer.ElapsedMilliseconds / 1000;
        
        if (!_isRecording && _timeBetweenNextSave > 0 && _numFramesBeforeNextSave > 0)
        {
            var amountOfTimeToAdd = (double)_timeBetweenNextSave / _numFramesBeforeNextSave;
            _currentFrameTime += amountOfTimeToAdd / 1000;
        }
        else if (!_isRecording && (_timeBetweenNextSave <= 0 || _numFramesBeforeNextSave <= 0))
        {
            _currentFrameTime = GetCurrentTime();
        }
        
        try
        {
            MemoryMappedFileUpdate();
            _mmfService.WriteTimerData(_currentFrameTime);

            _hasTimerStarted = forceTimerOnCheckbox.Checked || _mmfService.GetTimerStarted();
            if (_timerStartedBackup == false && _hasTimerStarted)
            {
                _timerStartedBackup = _hasTimerStarted;
                _currentFrameTime = 0.0;
                _startFrame = PokeAByteMainForm.Emulator.Frame;
                _isRunOver = false;
                _stopwatchTimer.Start();
                _shouldSaveState = true;
                SaveState(true, $"Run Started");
            }
            else if (_timerStartedBackup && !_hasTimerStarted)
            {
                _stopwatchTimer.Stop();
                _timerStartedBackup = _hasTimerStarted;
                _isRunOver = true;
                _shouldSaveState = true;
                var time = TimeSpan.FromSeconds(_currentFrameTime);
                SaveState(true, $"Run Ended - Total Time: {time.Hours}:{time.Minutes}:{time.Seconds}.{time.Milliseconds} - " +
                               $"Total Frames: {PokeAByteMainForm.Emulator.Frame - _startFrame}");
                _currentFrameTime = 0.0;
            }
        }
        catch (Exception ex)
        {
            statusLabel.Text = $"Error: {ex.Message}";
        }

        if (_saveName is not null && !string.IsNullOrWhiteSpace(_saveName))
        {
            SaveState(true, _saveName);
            _saveName = null;
        }
        else
        {
            SaveState();
        }

        ScrubMovie();
        ReloadState();
        
        if(!_isRecording)
            DrawDebugLabels();

        if (_pokeAByteMovieService.IsPlaybackOrComplete(PokeAByteMainForm, replayFileTextBox.Text) is true)
        {
            if (_pokeAByteMovieService.IsMovieComplete(PokeAByteMainForm, replayFileTextBox.Text) is true)
            {
                stopRecordingButton_Click(this, EventArgs.Empty);
            }
        }
        
        base.UpdateAfter();
    }
    
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _pipeService.Dispose();
            _saveStateTimer.Elapsed -= OnSaveStateTimerElapsedHandler;
            _saveStateTimer?.Dispose();
        }
        base.Dispose(disposing);
    }

}