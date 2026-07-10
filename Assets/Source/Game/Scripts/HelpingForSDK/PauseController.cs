using System.Collections.Generic;
using System.Linq;
using UnityEngine;

internal class PauseController
{
    private readonly HashSet<string> _pauseSourceKeys = new();
    private readonly HashSet<string> _soundSourceKeys = new();

    private const string MenuPauseKey = "WindowPause";
    private const float PlayValue = 1;
    private const float PauseValue = 0;

    internal void AddPauseSourceKey(string sourceKey)
    {
        AddSoundSourceKey(sourceKey);

        _pauseSourceKeys.Add(sourceKey);

        SetGamePlayback(false);
    }

    internal void RemovePauseSourceKey(string sourceKey)
    {
        RemoveSoundSourceKey(sourceKey);
        _pauseSourceKeys.Remove(_pauseSourceKeys.FirstOrDefault(key => key == sourceKey));

        if (_pauseSourceKeys.Count > 0)
            return;

        SetGamePlayback(true);
    }

    private void SetGamePlayback(bool value)
    {
        float timeScale = value ? PlayValue : PauseValue;

        Time.timeScale = timeScale;
    }

    private void AddSoundSourceKey(string sourceKey)
    {
        if (sourceKey == MenuPauseKey)
            return;

        _soundSourceKeys.Add(sourceKey);

        SetSoundPlayback(false);
    }

    private void RemoveSoundSourceKey(string sourceKey)
    {
        if (sourceKey == MenuPauseKey)
            return;

        _soundSourceKeys.Remove(_soundSourceKeys.FirstOrDefault(key => key == sourceKey));

        if (_soundSourceKeys.Count > 0)
            return;

        SetSoundPlayback(true);
    }

    private void SetSoundPlayback(bool value)
    {
        AudioListener.pause = !value;
    }
}