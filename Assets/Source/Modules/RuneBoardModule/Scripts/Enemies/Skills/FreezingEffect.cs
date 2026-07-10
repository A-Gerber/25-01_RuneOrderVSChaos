using System;
using System.Collections;
using UnityEngine;

public class FreezingEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem _effect;
    [SerializeField] private float _effectTime = 1.5f;

    private WaitForSeconds _wait;

    public event Action<FreezingEffect> Released;

    private void Awake()
    {
        _wait = new WaitForSeconds(_effectTime);
    }

    public void Perform()
    {
        _effect.Play();

        StartCoroutine(ReleaseOverTime());
    }

    private IEnumerator ReleaseOverTime()
    {
        yield return _wait;
        Released?.Invoke(this);
    }
}