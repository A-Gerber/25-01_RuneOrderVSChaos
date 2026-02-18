using System;
using System.Collections;
using UnityEngine;

internal class EffectConfetti : MonoBehaviour
{
    [SerializeField] private ParticleSystem _effect;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private float _delay = 2f;
    [SerializeField] private float _step = 0.15f;

    private WaitForSeconds _wait;
    private WaitForSeconds _waitBeforeStep;

    internal event Action<EffectConfetti> Released;

    private void Awake()
    {
        _wait = new WaitForSeconds(_delay);
        _waitBeforeStep = new WaitForSeconds(_step);
    }

    internal void Play(int number)
    {
        StartCoroutine(PlayOverTime(number));
        StartCoroutine(ReleaseOverTime());
    }

    private IEnumerator PlayOverTime(int countSteps)
    {
        for (int i = 0; i < countSteps; i++)
            yield return _waitBeforeStep;

        _effect.Play();
        _audioSource.Play();
    }

    private IEnumerator ReleaseOverTime()
    {
        yield return _wait;
        _effect.Stop();
        _audioSource.Stop();
        Released?.Invoke(this);
    }
}
