using System.Collections.Generic;
using System.Linq;
using UnityEngine;

internal class PauseController
{
    private readonly HashSet<string> _pauseSourceKeys = new();

    private const float PlayValue = 1;
    private const float PauseValue = 0;

    internal void AddPauseSourceKey(string sourceKey)
    {
        _pauseSourceKeys.Add(sourceKey);

        SetGamePlayback(false);
    }

    internal void RemovePauseSourceKey(string sourceKey)
    {
        _pauseSourceKeys.Remove(_pauseSourceKeys.FirstOrDefault(key => key == sourceKey));

        if (_pauseSourceKeys.Count > 0) 
            return;

        SetGamePlayback(true);
    }

    internal void SetSoundPlayback(bool value)
    {
        AudioListener.pause = value;
    }

    private void SetGamePlayback(bool value)
    {
        float timeScale = value ? PlayValue : PauseValue;

        Time.timeScale = timeScale;
    }
}