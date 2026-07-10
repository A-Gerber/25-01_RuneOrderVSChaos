using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
internal class WizardProjectileSoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip _createSound;
    [SerializeField] private AudioClip _damageSound;
    [SerializeField] private float _delay = 0.1f;

    private AudioSource _audioSource;
    private WaitForSeconds _wait;
    private bool _canPlaySound = true;

    private void Awake()
    {
        _wait = new WaitForSeconds(_delay);
        _audioSource = GetComponent<AudioSource>();
    }

    internal void PlayCreateSound()
    {
        _audioSource.PlayOneShot(_createSound);
    }

    internal void PlayDamageSound()
    {
        if (_canPlaySound)
            StartCoroutine(PlaySound());
    }

    private IEnumerator PlaySound()
    {
        _audioSource.PlayOneShot(_damageSound);
        _canPlaySound = false;
        yield return _wait;
        _canPlaySound = true;
    }
}