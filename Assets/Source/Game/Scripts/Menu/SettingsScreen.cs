using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

internal class SettingsScreen : Window
{
    private const float ValueMute = 0f;
    private const float ValueWithSound = 1f;
    private const float ValueWithSoundForMusic = 0.65f;
    private const string MusicParametrName = "MusicVolume";
    private const string SoundEffectParametrName = "EffectSounds";

    [SerializeField] private Toggle _soundToggle;
    [SerializeField] private Toggle _musicToggle;
    [SerializeField] private Toggle _soundEffectToggle;
    [SerializeField] private AudioMixerGroup _mixer;

    internal event Action ExitButtonClicked;

    protected override void OnEnable()
    {
        base.OnEnable();

        _soundToggle.onValueChanged.AddListener(ToggleSound);
        _musicToggle.onValueChanged.AddListener(ToggleMusic);
        _soundEffectToggle.onValueChanged.AddListener(ToggleEffectSounds);
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        _soundToggle.onValueChanged.RemoveListener(ToggleSound);
        _musicToggle.onValueChanged.RemoveListener(ToggleSound);
        _soundEffectToggle.onValueChanged.RemoveListener(ToggleSound);
    }

    protected override void OnButtonClick()
    {
        ExitButtonClicked?.Invoke();
    }

    private void ToggleSound(bool isDisabled)
    {
        if (isDisabled)
        {
            AudioListener.volume = ValueMute;
        }
        else
        {
            AudioListener.volume = ValueWithSound;
        }
    }


    private void ToggleEffectSounds(bool isEnabled)
    {
        if (isEnabled)
            _mixer.audioMixer.SetFloat(SoundEffectParametrName, UserUtilities.CalculateVolumeValue(ValueWithSound));
        else
            _mixer.audioMixer.SetFloat(SoundEffectParametrName, UserUtilities.CalculateVolumeValue(ValueMute));
    }

    private void ToggleMusic(bool isEnabled)
    {
        if (isEnabled)
            _mixer.audioMixer.SetFloat(MusicParametrName, UserUtilities.CalculateVolumeValue(ValueWithSoundForMusic));
        else
            _mixer.audioMixer.SetFloat(MusicParametrName, UserUtilities.CalculateVolumeValue(ValueMute));
    }
}