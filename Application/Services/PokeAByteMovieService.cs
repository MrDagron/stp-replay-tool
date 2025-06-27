using System;
using System.IO;
using System.Linq;
using BizHawk.Client.Common;
using BizHawk.Client.EmuHawk;
using BizHawk.Emulation.Common;

namespace PokeAByte.BizHawk.StpTool.Application.Services;

public class PokeAByteMovieService
{
    private IMovie? _currentMovie;
    #region New Movie
    public bool StartNewMovie(MainForm mainForm, string moviePath)
    {
        var path = CreateMoviePath(mainForm, moviePath);
        if (path is null) return false;
        var movieToRecord = mainForm.MovieSession.Get(path);
        var fileInfo = new FileInfo(path);
        if (!fileInfo.Exists)
        {
            Directory.CreateDirectory(fileInfo.DirectoryName);
        }
        InitMovie(mainForm, movieToRecord);
        var recording = mainForm.StartNewMovie(movieToRecord, true);
        return recording;
    }

    private string? CreateMoviePath(MainForm mainForm, string moviePath)
    {
        if (string.IsNullOrWhiteSpace(moviePath))
            return null; //todo: let user know they need to enter path
        var path = mainForm.Config is null ? moviePath : CreatePath(moviePath, mainForm.Config);
        if (string.IsNullOrWhiteSpace(path)) return null;
        var test = new FileInfo(path);
        if (!test.Exists) return path;
        var result = mainForm
            .DialogController
            .ShowMessageBox2($"{path} already exists, overwrite?", 
                "Confirm overwrite",
                EMsgBoxIcon.Warning,
                useOKCancel: true);
        return !result ? null : path;
    }
    private static string CreatePath(string path, Config config)
    {
        if (!string.IsNullOrWhiteSpace(path))
        {
            if (path.LastIndexOf(Path.DirectorySeparatorChar) == -1)
            {
                if (path[0] != Path.DirectorySeparatorChar)
                {
                    path = path.Insert(0, Path.DirectorySeparatorChar.ToString());
                }

                path = config.PathEntries.MovieAbsolutePath() + path;

                
                if (!global::BizHawk.Client.Common.MovieService.MovieExtensions.Contains(Path.GetExtension(path)))
                {
                    var stdMovieExt = global::BizHawk.Client.Common.MovieService.MovieExtensions;
                    // If no valid movie extension, add movie extension
                    path += $".{stdMovieExt}";
                }
            }
        }

        return path;
    }

    private void InitMovie(MainForm mainForm, IMovie movieToRecord)
    {
        var core = mainForm.Emulator.AsStatable();
        movieToRecord.StartsFromSavestate = true;
        if (mainForm.Config?.Savestates.Type == SaveStateType.Binary)
        {
            movieToRecord.BinarySavestate = core.CloneSavestate();
        }
        else
        {
            using var sw = new StringWriter();
            core.SaveStateText(sw);
            movieToRecord.TextSavestate = sw.ToString();
        }
        movieToRecord.SavestateFramebuffer = Array.Empty<int>();
        if (mainForm.Emulator.HasVideoProvider())
        {
            movieToRecord.SavestateFramebuffer = mainForm.Emulator.AsVideoProvider().GetVideoBufferCopy();
        }
        movieToRecord.PopulateWithDefaultHeaderValues(
            mainForm.Emulator,
            mainForm.GetSettingsAdapterForLoadedCoreUntyped(), //HACK
            mainForm.Game,
            mainForm.FirmwareManager,
            "");
    }
    #endregion
    #region Play Movie

    public void PlayMovie(MainForm mainForm, string? path)
    {
        var movie = mainForm.MovieSession.Get(path, true);
        movie.StartsFromSavestate = true;
        mainForm.StartNewMovie(movie, false);
        _currentMovie = movie;
    }
    public void StopMovie(MainForm mainForm, bool saveChanges = true)
    {
        mainForm.StopMovie(saveChanges);
        _currentMovie = null;
    }
    #endregion

    public bool? IsPlaybackOrComplete(MainForm mainForm, string? path)
    {
        if (_currentMovie is null || string.IsNullOrWhiteSpace(path))
            return null;
        var movie = _currentMovie ?? mainForm.MovieSession.Get(path);
        return movie?.IsPlayingOrFinished();
    }
    public bool? IsMovieComplete(MainForm mainForm, string? path)
    {        
        if (_currentMovie is null || string.IsNullOrWhiteSpace(path))
            return null;
        var movie = _currentMovie ?? mainForm.MovieSession.Get(path);
        return movie?.IsFinished();
    }
    public bool? IsMoviePlayback(MainForm mainForm, string? path)
    {
        if (_currentMovie is null || string.IsNullOrWhiteSpace(path))
            return null;
        var movie = _currentMovie ?? mainForm.MovieSession.Get(path);
        return movie?.IsPlaying();
    }
    public double GetFps()
    {
        return _currentMovie?.FrameRate ?? 0;
    }
}