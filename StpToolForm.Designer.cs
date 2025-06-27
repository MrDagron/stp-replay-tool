namespace PokeAByte.BizHawk.StpTool;

public partial class StpToolForm
{
    private System.Windows.Forms.Label statusLabel;
    private System.Windows.Forms.GroupBox savesGroupBox;
    private System.Windows.Forms.ListBox flaggedSavesListBox;
    private System.Windows.Forms.Label flagSavesLabel;
    private System.Windows.Forms.Label savesLabel;
    private System.Windows.Forms.Button replayMovieBrowse;
    private System.Windows.Forms.Button stopRecordingButton;
    private System.Windows.Forms.Button startRecordingButton;
    private System.Windows.Forms.TextBox replayFileTextBox;
    private System.Windows.Forms.Button renameSaveButton;
    private System.Windows.Forms.Label replayMovieLoadedLabel;
    private System.Windows.Forms.ListBox flagListBox;
    private System.Windows.Forms.Button flagsFileLoadButton;
    private System.Windows.Forms.Label flagsLoadedLabel;
    private System.Windows.Forms.Button flagsFileBrowseButton;
    private System.Windows.Forms.TextBox flagFileTextBox;
    private System.Windows.Forms.GroupBox movieGroupBox;


    private void InitializeComponent()
    {
            this.statusLabel = new System.Windows.Forms.Label();
            this.movieGroupBox = new System.Windows.Forms.GroupBox();
            this.replayDirectoryLabel = new System.Windows.Forms.Label();
            this.replayMovieLoadedLabel = new System.Windows.Forms.Label();
            this.replayMovieBrowse = new System.Windows.Forms.Button();
            this.replayFileTextBox = new System.Windows.Forms.TextBox();
            this.movieReplayPauseButton = new System.Windows.Forms.Button();
            this.moviePlayButton = new System.Windows.Forms.Button();
            this.stopRecordingButton = new System.Windows.Forms.Button();
            this.startRecordingButton = new System.Windows.Forms.Button();
            this.flagListBox = new System.Windows.Forms.ListBox();
            this.flagsFileLoadButton = new System.Windows.Forms.Button();
            this.flagsLoadedLabel = new System.Windows.Forms.Label();
            this.flagsFileBrowseButton = new System.Windows.Forms.Button();
            this.flagFileTextBox = new System.Windows.Forms.TextBox();
            this.savesGroupBox = new System.Windows.Forms.GroupBox();
            this.savesListView = new System.Windows.Forms.ListView();
            this.deleteSaveButton = new System.Windows.Forms.Button();
            this.renameSaveButton = new System.Windows.Forms.Button();
            this.flaggedSavesListBox = new System.Windows.Forms.ListBox();
            this.flagSavesLabel = new System.Windows.Forms.Label();
            this.savesLabel = new System.Windows.Forms.Label();
            this.saveFrameButton = new System.Windows.Forms.Button();
            this.loadFrameButton = new System.Windows.Forms.Button();
            this.debuggingBox = new System.Windows.Forms.GroupBox();
            this.forceTimerOnCheckbox = new System.Windows.Forms.CheckBox();
            this.fpsLabel = new System.Windows.Forms.Label();
            this.currentKeyLabel = new System.Windows.Forms.Label();
            this.stopwatchTimeLabel = new System.Windows.Forms.Label();
            this.saveIntervalText = new System.Windows.Forms.NumericUpDown();
            this.saveSaveIntervalButton = new System.Windows.Forms.Button();
            this.ignoreTimerCheckbox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.mousePositionLabel = new System.Windows.Forms.Label();
            this.currentSystemLabel = new System.Windows.Forms.Label();
            this.currentGameLabel = new System.Windows.Forms.Label();
            this.currentFrameLabel = new System.Windows.Forms.Label();
            this.timeScrubber1 = new PokeAByte.BizHawk.StpTool.TimeScrubber();
            this.flagsGroupBox = new System.Windows.Forms.GroupBox();
            this.flagsFileListBox = new System.Windows.Forms.ListBox();
            this.browseFlagsFileButton = new System.Windows.Forms.Button();
            this.flagsLabel = new System.Windows.Forms.Label();
            this.flagsFileTextBox = new System.Windows.Forms.TextBox();
            this.playCollectButton = new System.Windows.Forms.Button();
            this.stateLabel = new System.Windows.Forms.Label();
            this.saveStatesButton = new System.Windows.Forms.Button();
            this.collectAfterSaveButton = new System.Windows.Forms.Button();
            this.deleteAfterSaveButton = new System.Windows.Forms.Button();
            this.movieGroupBox.SuspendLayout();
            this.savesGroupBox.SuspendLayout();
            this.debuggingBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.saveIntervalText)).BeginInit();
            this.flagsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.statusLabel.Location = new System.Drawing.Point(0, 0);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(69, 13);
            this.statusLabel.TabIndex = 0;
            this.statusLabel.Text = "{statusLabel}";
            // 
            // movieGroupBox
            // 
            this.movieGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.movieGroupBox.Controls.Add(this.replayDirectoryLabel);
            this.movieGroupBox.Controls.Add(this.replayMovieLoadedLabel);
            this.movieGroupBox.Controls.Add(this.replayMovieBrowse);
            this.movieGroupBox.Controls.Add(this.replayFileTextBox);
            this.movieGroupBox.Location = new System.Drawing.Point(3, 16);
            this.movieGroupBox.Name = "movieGroupBox";
            this.movieGroupBox.Size = new System.Drawing.Size(413, 77);
            this.movieGroupBox.TabIndex = 1;
            this.movieGroupBox.TabStop = false;
            this.movieGroupBox.Text = "Movie";
            // 
            // replayDirectoryLabel
            // 
            this.replayDirectoryLabel.AutoSize = true;
            this.replayDirectoryLabel.Location = new System.Drawing.Point(9, 16);
            this.replayDirectoryLabel.Name = "replayDirectoryLabel";
            this.replayDirectoryLabel.Size = new System.Drawing.Size(85, 13);
            this.replayDirectoryLabel.TabIndex = 7;
            this.replayDirectoryLabel.Text = "Replay Directory";
            // 
            // replayMovieLoadedLabel
            // 
            this.replayMovieLoadedLabel.AutoSize = true;
            this.replayMovieLoadedLabel.Location = new System.Drawing.Point(10, 56);
            this.replayMovieLoadedLabel.Name = "replayMovieLoadedLabel";
            this.replayMovieLoadedLabel.Size = new System.Drawing.Size(139, 13);
            this.replayMovieLoadedLabel.TabIndex = 4;
            this.replayMovieLoadedLabel.Text = "{ReplayMovieLoadedLabel}";
            // 
            // replayMovieBrowse
            // 
            this.replayMovieBrowse.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.replayMovieBrowse.Location = new System.Drawing.Point(332, 33);
            this.replayMovieBrowse.Name = "replayMovieBrowse";
            this.replayMovieBrowse.Size = new System.Drawing.Size(75, 23);
            this.replayMovieBrowse.TabIndex = 3;
            this.replayMovieBrowse.Text = "Browse";
            this.replayMovieBrowse.UseVisualStyleBackColor = true;
            this.replayMovieBrowse.Click += new System.EventHandler(this.replayMovieBrowse_Click);
            // 
            // replayFileTextBox
            // 
            this.replayFileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.replayFileTextBox.Location = new System.Drawing.Point(13, 33);
            this.replayFileTextBox.Name = "replayFileTextBox";
            this.replayFileTextBox.ReadOnly = true;
            this.replayFileTextBox.Size = new System.Drawing.Size(314, 20);
            this.replayFileTextBox.TabIndex = 0;
            this.replayFileTextBox.Text = "{ReplayFileTextBox}";
            // 
            // movieReplayPauseButton
            // 
            this.movieReplayPauseButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.movieReplayPauseButton.Location = new System.Drawing.Point(0, 742);
            this.movieReplayPauseButton.Name = "movieReplayPauseButton";
            this.movieReplayPauseButton.Size = new System.Drawing.Size(81, 23);
            this.movieReplayPauseButton.TabIndex = 9;
            this.movieReplayPauseButton.Text = "Pause Emu";
            this.movieReplayPauseButton.UseVisualStyleBackColor = true;
            this.movieReplayPauseButton.Click += new System.EventHandler(this.movieReplayPauseButton_Click);
            // 
            // moviePlayButton
            // 
            this.moviePlayButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.moviePlayButton.Location = new System.Drawing.Point(0, 771);
            this.moviePlayButton.Name = "moviePlayButton";
            this.moviePlayButton.Size = new System.Drawing.Size(81, 23);
            this.moviePlayButton.TabIndex = 8;
            this.moviePlayButton.Text = "Play Movie";
            this.moviePlayButton.UseVisualStyleBackColor = true;
            this.moviePlayButton.Click += new System.EventHandler(this.moviePlayButton_Click);
            // 
            // stopRecordingButton
            // 
            this.stopRecordingButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.stopRecordingButton.Location = new System.Drawing.Point(87, 771);
            this.stopRecordingButton.Name = "stopRecordingButton";
            this.stopRecordingButton.Size = new System.Drawing.Size(107, 23);
            this.stopRecordingButton.TabIndex = 2;
            this.stopRecordingButton.Text = "Take Control";
            this.stopRecordingButton.UseVisualStyleBackColor = true;
            this.stopRecordingButton.Click += new System.EventHandler(this.stopRecordingButton_Click);
            // 
            // startRecordingButton
            // 
            this.startRecordingButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.startRecordingButton.Location = new System.Drawing.Point(87, 742);
            this.startRecordingButton.Name = "startRecordingButton";
            this.startRecordingButton.Size = new System.Drawing.Size(107, 23);
            this.startRecordingButton.TabIndex = 1;
            this.startRecordingButton.Text = "Start Record Movie";
            this.startRecordingButton.UseVisualStyleBackColor = true;
            this.startRecordingButton.Click += new System.EventHandler(this.startRecordingButton_Click);
            // 
            // flagListBox
            // 
            this.flagListBox.Location = new System.Drawing.Point(0, 0);
            this.flagListBox.Name = "flagListBox";
            this.flagListBox.Size = new System.Drawing.Size(120, 96);
            this.flagListBox.TabIndex = 0;
            // 
            // flagsFileLoadButton
            // 
            this.flagsFileLoadButton.Location = new System.Drawing.Point(0, 0);
            this.flagsFileLoadButton.Name = "flagsFileLoadButton";
            this.flagsFileLoadButton.Size = new System.Drawing.Size(75, 23);
            this.flagsFileLoadButton.TabIndex = 0;
            // 
            // flagsLoadedLabel
            // 
            this.flagsLoadedLabel.Location = new System.Drawing.Point(0, 0);
            this.flagsLoadedLabel.Name = "flagsLoadedLabel";
            this.flagsLoadedLabel.Size = new System.Drawing.Size(100, 23);
            this.flagsLoadedLabel.TabIndex = 0;
            // 
            // flagsFileBrowseButton
            // 
            this.flagsFileBrowseButton.Location = new System.Drawing.Point(0, 0);
            this.flagsFileBrowseButton.Name = "flagsFileBrowseButton";
            this.flagsFileBrowseButton.Size = new System.Drawing.Size(75, 23);
            this.flagsFileBrowseButton.TabIndex = 0;
            // 
            // flagFileTextBox
            // 
            this.flagFileTextBox.Location = new System.Drawing.Point(0, 0);
            this.flagFileTextBox.Name = "flagFileTextBox";
            this.flagFileTextBox.Size = new System.Drawing.Size(100, 20);
            this.flagFileTextBox.TabIndex = 0;
            // 
            // savesGroupBox
            // 
            this.savesGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.savesGroupBox.Controls.Add(this.savesListView);
            this.savesGroupBox.Controls.Add(this.deleteSaveButton);
            this.savesGroupBox.Controls.Add(this.renameSaveButton);
            this.savesGroupBox.Controls.Add(this.flaggedSavesListBox);
            this.savesGroupBox.Controls.Add(this.flagSavesLabel);
            this.savesGroupBox.Controls.Add(this.savesLabel);
            this.savesGroupBox.Location = new System.Drawing.Point(3, 99);
            this.savesGroupBox.Name = "savesGroupBox";
            this.savesGroupBox.Size = new System.Drawing.Size(413, 331);
            this.savesGroupBox.TabIndex = 3;
            this.savesGroupBox.TabStop = false;
            this.savesGroupBox.Text = "Saves";
            // 
            // savesListView
            // 
            this.savesListView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.savesListView.FullRowSelect = true;
            this.savesListView.HideSelection = false;
            this.savesListView.Location = new System.Drawing.Point(6, 32);
            this.savesListView.Name = "savesListView";
            this.savesListView.Size = new System.Drawing.Size(390, 121);
            this.savesListView.TabIndex = 6;
            this.savesListView.UseCompatibleStateImageBehavior = false;
            this.savesListView.View = System.Windows.Forms.View.Details;
            this.savesListView.VirtualListSize = 10000;
            this.savesListView.VirtualMode = true;
            // 
            // deleteSaveButton
            // 
            this.deleteSaveButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.deleteSaveButton.Location = new System.Drawing.Point(243, 6);
            this.deleteSaveButton.Name = "deleteSaveButton";
            this.deleteSaveButton.Size = new System.Drawing.Size(75, 23);
            this.deleteSaveButton.TabIndex = 5;
            this.deleteSaveButton.Text = "Delete";
            this.deleteSaveButton.UseVisualStyleBackColor = true;
            this.deleteSaveButton.Click += new System.EventHandler(this.deleteSaveButton_Click);
            // 
            // renameSaveButton
            // 
            this.renameSaveButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.renameSaveButton.Location = new System.Drawing.Point(321, 6);
            this.renameSaveButton.Name = "renameSaveButton";
            this.renameSaveButton.Size = new System.Drawing.Size(75, 23);
            this.renameSaveButton.TabIndex = 4;
            this.renameSaveButton.Text = "Rename";
            this.renameSaveButton.UseVisualStyleBackColor = true;
            this.renameSaveButton.Click += new System.EventHandler(this.renameSaveButton_Click);
            // 
            // flaggedSavesListBox
            // 
            this.flaggedSavesListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.flaggedSavesListBox.FormattingEnabled = true;
            this.flaggedSavesListBox.Location = new System.Drawing.Point(6, 185);
            this.flaggedSavesListBox.Name = "flaggedSavesListBox";
            this.flaggedSavesListBox.ScrollAlwaysVisible = true;
            this.flaggedSavesListBox.Size = new System.Drawing.Size(390, 121);
            this.flaggedSavesListBox.TabIndex = 3;
            this.flaggedSavesListBox.SelectedIndexChanged += new System.EventHandler(this.flaggedSavesListBox_SelectedIndexChanged);
            // 
            // flagSavesLabel
            // 
            this.flagSavesLabel.AutoSize = true;
            this.flagSavesLabel.Location = new System.Drawing.Point(3, 169);
            this.flagSavesLabel.Name = "flagSavesLabel";
            this.flagSavesLabel.Size = new System.Drawing.Size(78, 13);
            this.flagSavesLabel.TabIndex = 2;
            this.flagSavesLabel.Text = "Flagged Saves";
            // 
            // savesLabel
            // 
            this.savesLabel.AutoSize = true;
            this.savesLabel.Location = new System.Drawing.Point(6, 16);
            this.savesLabel.Name = "savesLabel";
            this.savesLabel.Size = new System.Drawing.Size(51, 13);
            this.savesLabel.TabIndex = 1;
            this.savesLabel.Text = "All Saves";
            // 
            // saveFrameButton
            // 
            this.saveFrameButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.saveFrameButton.Location = new System.Drawing.Point(243, 19);
            this.saveFrameButton.Name = "saveFrameButton";
            this.saveFrameButton.Size = new System.Drawing.Size(75, 23);
            this.saveFrameButton.TabIndex = 4;
            this.saveFrameButton.Text = "Test 1";
            this.saveFrameButton.UseVisualStyleBackColor = true;
            this.saveFrameButton.Click += new System.EventHandler(this.saveFrameButton_Click);
            // 
            // loadFrameButton
            // 
            this.loadFrameButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.loadFrameButton.Location = new System.Drawing.Point(318, 19);
            this.loadFrameButton.Name = "loadFrameButton";
            this.loadFrameButton.Size = new System.Drawing.Size(75, 23);
            this.loadFrameButton.TabIndex = 5;
            this.loadFrameButton.Text = "Test 2";
            this.loadFrameButton.UseVisualStyleBackColor = true;
            this.loadFrameButton.Click += new System.EventHandler(this.loadFrameButton_Click);
            // 
            // debuggingBox
            // 
            this.debuggingBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.debuggingBox.Controls.Add(this.forceTimerOnCheckbox);
            this.debuggingBox.Controls.Add(this.fpsLabel);
            this.debuggingBox.Controls.Add(this.currentKeyLabel);
            this.debuggingBox.Controls.Add(this.stopwatchTimeLabel);
            this.debuggingBox.Controls.Add(this.saveIntervalText);
            this.debuggingBox.Controls.Add(this.saveSaveIntervalButton);
            this.debuggingBox.Controls.Add(this.ignoreTimerCheckbox);
            this.debuggingBox.Controls.Add(this.label1);
            this.debuggingBox.Controls.Add(this.mousePositionLabel);
            this.debuggingBox.Controls.Add(this.currentSystemLabel);
            this.debuggingBox.Controls.Add(this.currentGameLabel);
            this.debuggingBox.Controls.Add(this.currentFrameLabel);
            this.debuggingBox.Controls.Add(this.saveFrameButton);
            this.debuggingBox.Controls.Add(this.loadFrameButton);
            this.debuggingBox.Location = new System.Drawing.Point(0, 628);
            this.debuggingBox.Name = "debuggingBox";
            this.debuggingBox.Size = new System.Drawing.Size(410, 101);
            this.debuggingBox.TabIndex = 7;
            this.debuggingBox.TabStop = false;
            this.debuggingBox.Text = "Debugging";
            // 
            // forceTimerOnCheckbox
            // 
            this.forceTimerOnCheckbox.AutoSize = true;
            this.forceTimerOnCheckbox.Location = new System.Drawing.Point(138, 84);
            this.forceTimerOnCheckbox.Name = "forceTimerOnCheckbox";
            this.forceTimerOnCheckbox.Size = new System.Drawing.Size(99, 17);
            this.forceTimerOnCheckbox.TabIndex = 17;
            this.forceTimerOnCheckbox.Text = "Force Timer On";
            this.forceTimerOnCheckbox.UseVisualStyleBackColor = true;
            // 
            // fpsLabel
            // 
            this.fpsLabel.AutoSize = true;
            this.fpsLabel.Location = new System.Drawing.Point(159, 16);
            this.fpsLabel.Name = "fpsLabel";
            this.fpsLabel.Size = new System.Drawing.Size(36, 13);
            this.fpsLabel.TabIndex = 16;
            this.fpsLabel.Text = "Fps: 0";
            // 
            // currentKeyLabel
            // 
            this.currentKeyLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.currentKeyLabel.AutoSize = true;
            this.currentKeyLabel.Location = new System.Drawing.Point(6, 81);
            this.currentKeyLabel.Name = "currentKeyLabel";
            this.currentKeyLabel.Size = new System.Drawing.Size(74, 13);
            this.currentKeyLabel.TabIndex = 15;
            this.currentKeyLabel.Text = "Current Key: 0";
            // 
            // stopwatchTimeLabel
            // 
            this.stopwatchTimeLabel.AutoSize = true;
            this.stopwatchTimeLabel.Location = new System.Drawing.Point(6, 16);
            this.stopwatchTimeLabel.Name = "stopwatchTimeLabel";
            this.stopwatchTimeLabel.Size = new System.Drawing.Size(79, 13);
            this.stopwatchTimeLabel.TabIndex = 14;
            this.stopwatchTimeLabel.Text = "Current Time: 0";
            // 
            // saveIntervalText
            // 
            this.saveIntervalText.Location = new System.Drawing.Point(243, 59);
            this.saveIntervalText.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.saveIntervalText.Name = "saveIntervalText";
            this.saveIntervalText.Size = new System.Drawing.Size(92, 20);
            this.saveIntervalText.TabIndex = 13;
            this.saveIntervalText.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // saveSaveIntervalButton
            // 
            this.saveSaveIntervalButton.Location = new System.Drawing.Point(339, 56);
            this.saveSaveIntervalButton.Name = "saveSaveIntervalButton";
            this.saveSaveIntervalButton.Size = new System.Drawing.Size(57, 23);
            this.saveSaveIntervalButton.TabIndex = 12;
            this.saveSaveIntervalButton.Text = "Save";
            this.saveSaveIntervalButton.UseVisualStyleBackColor = true;
            this.saveSaveIntervalButton.Click += new System.EventHandler(this.saveSaveIntervalButton_Click);
            // 
            // ignoreTimerCheckbox
            // 
            this.ignoreTimerCheckbox.AutoSize = true;
            this.ignoreTimerCheckbox.Location = new System.Drawing.Point(243, 83);
            this.ignoreTimerCheckbox.Name = "ignoreTimerCheckbox";
            this.ignoreTimerCheckbox.Size = new System.Drawing.Size(140, 17);
            this.ignoreTimerCheckbox.TabIndex = 12;
            this.ignoreTimerCheckbox.Text = "Disable Saving Intervals";
            this.ignoreTimerCheckbox.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(243, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Save Interval";
            // 
            // mousePositionLabel
            // 
            this.mousePositionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.mousePositionLabel.AutoSize = true;
            this.mousePositionLabel.Location = new System.Drawing.Point(6, 29);
            this.mousePositionLabel.Name = "mousePositionLabel";
            this.mousePositionLabel.Size = new System.Drawing.Size(106, 13);
            this.mousePositionLabel.TabIndex = 9;
            this.mousePositionLabel.Text = "Mouse Position: (0,0)";
            // 
            // currentSystemLabel
            // 
            this.currentSystemLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.currentSystemLabel.AutoSize = true;
            this.currentSystemLabel.Location = new System.Drawing.Point(6, 55);
            this.currentSystemLabel.Name = "currentSystemLabel";
            this.currentSystemLabel.Size = new System.Drawing.Size(81, 13);
            this.currentSystemLabel.TabIndex = 8;
            this.currentSystemLabel.Text = "Current System:";
            // 
            // currentGameLabel
            // 
            this.currentGameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.currentGameLabel.AutoSize = true;
            this.currentGameLabel.Location = new System.Drawing.Point(6, 42);
            this.currentGameLabel.Name = "currentGameLabel";
            this.currentGameLabel.Size = new System.Drawing.Size(78, 13);
            this.currentGameLabel.TabIndex = 7;
            this.currentGameLabel.Text = "Current Game: ";
            // 
            // currentFrameLabel
            // 
            this.currentFrameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.currentFrameLabel.AutoSize = true;
            this.currentFrameLabel.Location = new System.Drawing.Point(6, 68);
            this.currentFrameLabel.Name = "currentFrameLabel";
            this.currentFrameLabel.Size = new System.Drawing.Size(85, 13);
            this.currentFrameLabel.TabIndex = 6;
            this.currentFrameLabel.Text = "Current Frame: 0";
            // 
            // timeScrubber1
            // 
            this.timeScrubber1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.timeScrubber1.BackColor = System.Drawing.Color.Gray;
            this.timeScrubber1.Location = new System.Drawing.Point(0, 821);
            this.timeScrubber1.Name = "timeScrubber1";
            this.timeScrubber1.Size = new System.Drawing.Size(413, 30);
            this.timeScrubber1.TabIndex = 8;
            this.timeScrubber1.Text = "timeScrubber1";
            // 
            // flagsGroupBox
            // 
            this.flagsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flagsGroupBox.Controls.Add(this.flagsFileListBox);
            this.flagsGroupBox.Controls.Add(this.browseFlagsFileButton);
            this.flagsGroupBox.Controls.Add(this.flagsLabel);
            this.flagsGroupBox.Controls.Add(this.flagsFileTextBox);
            this.flagsGroupBox.Location = new System.Drawing.Point(0, 436);
            this.flagsGroupBox.Name = "flagsGroupBox";
            this.flagsGroupBox.Size = new System.Drawing.Size(413, 186);
            this.flagsGroupBox.TabIndex = 9;
            this.flagsGroupBox.TabStop = false;
            this.flagsGroupBox.Text = "Flags";
            // 
            // flagsFileListBox
            // 
            this.flagsFileListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.flagsFileListBox.FormattingEnabled = true;
            this.flagsFileListBox.Location = new System.Drawing.Point(6, 60);
            this.flagsFileListBox.Name = "flagsFileListBox";
            this.flagsFileListBox.ScrollAlwaysVisible = true;
            this.flagsFileListBox.Size = new System.Drawing.Size(390, 108);
            this.flagsFileListBox.TabIndex = 3;
            // 
            // browseFlagsFileButton
            // 
            this.browseFlagsFileButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.browseFlagsFileButton.Location = new System.Drawing.Point(332, 31);
            this.browseFlagsFileButton.Name = "browseFlagsFileButton";
            this.browseFlagsFileButton.Size = new System.Drawing.Size(75, 23);
            this.browseFlagsFileButton.TabIndex = 2;
            this.browseFlagsFileButton.Text = "Browse";
            this.browseFlagsFileButton.UseVisualStyleBackColor = true;
            this.browseFlagsFileButton.Click += new System.EventHandler(this.browseFlagsFileButton_Click);
            // 
            // flagsLabel
            // 
            this.flagsLabel.AutoSize = true;
            this.flagsLabel.Location = new System.Drawing.Point(3, 18);
            this.flagsLabel.Name = "flagsLabel";
            this.flagsLabel.Size = new System.Drawing.Size(51, 13);
            this.flagsLabel.TabIndex = 1;
            this.flagsLabel.Text = "Flags File";
            // 
            // flagsFileTextBox
            // 
            this.flagsFileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.flagsFileTextBox.Location = new System.Drawing.Point(6, 34);
            this.flagsFileTextBox.Name = "flagsFileTextBox";
            this.flagsFileTextBox.ReadOnly = true;
            this.flagsFileTextBox.Size = new System.Drawing.Size(320, 20);
            this.flagsFileTextBox.TabIndex = 0;
            // 
            // playCollectButton
            // 
            this.playCollectButton.Location = new System.Drawing.Point(200, 742);
            this.playCollectButton.Name = "playCollectButton";
            this.playCollectButton.Size = new System.Drawing.Size(103, 23);
            this.playCollectButton.TabIndex = 10;
            this.playCollectButton.Text = "Play/Collect";
            this.playCollectButton.UseVisualStyleBackColor = true;
            this.playCollectButton.Click += new System.EventHandler(this.playCollectButton_Click);
            // 
            // stateLabel
            // 
            this.stateLabel.AutoSize = true;
            this.stateLabel.Location = new System.Drawing.Point(0, 801);
            this.stateLabel.Name = "stateLabel";
            this.stateLabel.Size = new System.Drawing.Size(38, 13);
            this.stateLabel.TabIndex = 11;
            this.stateLabel.Text = "{state}";
            // 
            // saveStatesButton
            // 
            this.saveStatesButton.Location = new System.Drawing.Point(309, 742);
            this.saveStatesButton.Name = "saveStatesButton";
            this.saveStatesButton.Size = new System.Drawing.Size(107, 23);
            this.saveStatesButton.TabIndex = 13;
            this.saveStatesButton.Text = "Save States";
            this.saveStatesButton.UseVisualStyleBackColor = true;
            this.saveStatesButton.Click += new System.EventHandler(this.saveStatesButton_Click);
            // 
            // collectAfterSaveButton
            // 
            this.collectAfterSaveButton.Location = new System.Drawing.Point(200, 771);
            this.collectAfterSaveButton.Name = "collectAfterSaveButton";
            this.collectAfterSaveButton.Size = new System.Drawing.Size(103, 23);
            this.collectAfterSaveButton.TabIndex = 14;
            this.collectAfterSaveButton.Text = "Collect After Save";
            this.collectAfterSaveButton.UseVisualStyleBackColor = true;
            this.collectAfterSaveButton.Click += new System.EventHandler(this.collectAfterSaveButton_Click);
            // 
            // deleteAfterSaveButton
            // 
            this.deleteAfterSaveButton.Location = new System.Drawing.Point(309, 771);
            this.deleteAfterSaveButton.Name = "deleteAfterSaveButton";
            this.deleteAfterSaveButton.Size = new System.Drawing.Size(104, 23);
            this.deleteAfterSaveButton.TabIndex = 15;
            this.deleteAfterSaveButton.Text = "Delete After Save";
            this.deleteAfterSaveButton.UseVisualStyleBackColor = true;
            this.deleteAfterSaveButton.Click += new System.EventHandler(this.deleteAfterSaveButton_Click);
            // 
            // StpToolForm
            // 
            this.ClientSize = new System.Drawing.Size(413, 853);
            this.Controls.Add(this.deleteAfterSaveButton);
            this.Controls.Add(this.collectAfterSaveButton);
            this.Controls.Add(this.saveStatesButton);
            this.Controls.Add(this.stateLabel);
            this.Controls.Add(this.playCollectButton);
            this.Controls.Add(this.moviePlayButton);
            this.Controls.Add(this.movieReplayPauseButton);
            this.Controls.Add(this.flagsGroupBox);
            this.Controls.Add(this.stopRecordingButton);
            this.Controls.Add(this.debuggingBox);
            this.Controls.Add(this.timeScrubber1);
            this.Controls.Add(this.startRecordingButton);
            this.Controls.Add(this.savesGroupBox);
            this.Controls.Add(this.movieGroupBox);
            this.Controls.Add(this.statusLabel);
            this.Name = "StpToolForm";
            this.movieGroupBox.ResumeLayout(false);
            this.movieGroupBox.PerformLayout();
            this.savesGroupBox.ResumeLayout(false);
            this.savesGroupBox.PerformLayout();
            this.debuggingBox.ResumeLayout(false);
            this.debuggingBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.saveIntervalText)).EndInit();
            this.flagsGroupBox.ResumeLayout(false);
            this.flagsGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

    }
    private System.Windows.Forms.Label replayDirectoryLabel;
    private System.Windows.Forms.Button saveFrameButton;
    private System.Windows.Forms.Button loadFrameButton;
    private System.Windows.Forms.Button moviePlayButton;
    private System.Windows.Forms.GroupBox debuggingBox;
    private System.Windows.Forms.Label currentFrameLabel;
    private System.Windows.Forms.Label currentGameLabel;
    private System.Windows.Forms.Label currentSystemLabel;
    private System.Windows.Forms.Button movieReplayPauseButton;
    private TimeScrubber timeScrubber1;
    private System.Windows.Forms.Label mousePositionLabel;
    private System.Windows.Forms.GroupBox flagsGroupBox;
    private System.Windows.Forms.ListBox flagsFileListBox;
    private System.Windows.Forms.Button browseFlagsFileButton;
    private System.Windows.Forms.Label flagsLabel;
    private System.Windows.Forms.TextBox flagsFileTextBox;
    private System.Windows.Forms.Button deleteSaveButton;
    private System.Windows.Forms.ListView savesListView;
    private System.Windows.Forms.Button playCollectButton;
    private System.Windows.Forms.Label stateLabel;
    private System.Windows.Forms.CheckBox ignoreTimerCheckbox;
    private System.Windows.Forms.Button saveStatesButton;
    private System.Windows.Forms.NumericUpDown saveIntervalText;
    private System.Windows.Forms.Button saveSaveIntervalButton;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label stopwatchTimeLabel;
    private System.Windows.Forms.Button collectAfterSaveButton;
    private System.Windows.Forms.Button deleteAfterSaveButton;
    private System.Windows.Forms.Label currentKeyLabel;
    private System.Windows.Forms.Label fpsLabel;
    private System.Windows.Forms.CheckBox forceTimerOnCheckbox;
}